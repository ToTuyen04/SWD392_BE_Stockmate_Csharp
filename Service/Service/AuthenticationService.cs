using Microsoft.IdentityModel.Tokens;
using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Repository.Interface;
using Service.Service.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Repository.Models.Enums;
using Repository.Models.Exceptions;

namespace Service.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _jwtSecret = "eaRge+NAiFb7HQITA/QcCaDmS7QXJlwy7UpOAJj5/ddqoWCYQquoPXkget8OK+zA"; // Should be in config
        private readonly string _issuer = "SWD392_BE_MOBILE";
        private readonly string _audience = "SWD392_BE_MOBILE_USERS";

        public AuthenticationService() => _unitOfWork = new Repository.Repository.UnitOfWork();

        public async Task<AuthenticationResponse> Authenticate(AuthenticationRequest request)
        {
            // Find user by email
            var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                throw new AppException(ErrorCode.EMAIL_NOT_EXIST);
            }

            // Verify password
            bool authenticated = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

            if (!authenticated)
            {
                throw new AppException(ErrorCode.UNAUTHENTICATED);
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);

            return new AuthenticationResponse
            {
                Token = token,
                Authenticated = true,
                UserCode = user.UserCode,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<IntrospectResponse> Introspect(IntrospectRequest request)
        {
            var token = request.Token;
            bool isValid = true;

            try
            {
                await VerifyToken(token);
            }
            catch (AppException)
            {
                isValid = false;
            }

            return new IntrospectResponse { Valid = isValid };
        }

        public async Task Logout(LogoutRequest request)
        {
            try
            {
                var signedToken = await VerifyToken(request.Token);

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(request.Token);

                var jti = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
                var exp = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)?.Value;

                if (!string.IsNullOrEmpty(jti) && !string.IsNullOrEmpty(exp))
                {
                    var expiryTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp)).DateTime;

                    await _unitOfWork.InvalidTokenRepository.CreateAsync(new Repository.Models.Entities.InvalidatedToken
                    {
                        Id = jti,
                        ExpiryTime = expiryTime
                    });
                }
            }
            catch (AppException)
            {
                // Token already expired, log info but don't throw
                Console.WriteLine("Token đã hết hạn");
            }
        }

        private string GenerateJwtToken(Repository.Models.Entities.User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserId),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("userCode", user.UserCode),
                    new Claim("role", user.Role?.RoleType ?? ""),
                    new Claim("scope", user.Role?.RoleType ?? "")
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task<JwtSecurityToken> VerifyToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                // Check if token is in invalidated tokens list
                var jti = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;

                if (!string.IsNullOrEmpty(jti))
                {
                    var invalidatedToken = await _unitOfWork.InvalidTokenRepository.GetByIdAsync(jti);
                    if (invalidatedToken != null)
                    {
                        throw new AppException(ErrorCode.UNAUTHENTICATED);
                    }
                }

                return jwtToken;
            }
            catch (SecurityTokenException)
            {
                throw new AppException(ErrorCode.UNAUTHENTICATED);
            }
            catch (ArgumentException)
            {
                throw new AppException(ErrorCode.UNAUTHENTICATED);
            }
        }
    }
}
