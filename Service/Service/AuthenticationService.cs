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
            try
            {
                // Find user by email
                var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);

                if (user == null)
                {
                    throw new UnauthorizedAccessException("Invalid email or password.");
                }

                // Verify password
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                {
                    throw new UnauthorizedAccessException("Invalid email or password.");
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
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during authentication.", ex);
            }
        }

        public async Task<IntrospectResponse> Introspect(IntrospectRequest request)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSecret);

                tokenHandler.ValidateToken(request.Token, new TokenValidationParameters
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

                // Check if token is in invalidated tokens list
                var jwtToken = (JwtSecurityToken)validatedToken;
                var jti = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;

                if (!string.IsNullOrEmpty(jti))
                {
                    var invalidatedToken = await _unitOfWork.InvalidTokenRepository.GetByIdAsync(jti);
                    if (invalidatedToken != null)
                    {
                        return new IntrospectResponse { Valid = false };
                    }
                }

                return new IntrospectResponse { Valid = true };
            }
            catch
            {
                return new IntrospectResponse { Valid = false };
            }
        }

        public async Task Logout(LogoutRequest request)
        {
            try
            {
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
            catch (Exception ex)
            {
                throw new Exception("An error occurred during logout.", ex);
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
    }
}
