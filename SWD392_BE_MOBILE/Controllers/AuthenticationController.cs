
using Microsoft.AspNetCore.Mvc;
using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Models.Exceptions;
using Repository.Models.Enums;
using Service.Service.Interface;

namespace SWD392_BE_MOBILE.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceProviders _serviceProviders;

        public AuthenticationController(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        /// <summary>
        /// Login endpoint - Authenticate user and return JWT token
        /// </summary>
        [HttpPost("token")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<AuthenticationResponse>
                {
                    Code = 400,
                    Message = "Invalid authentication data.",
                    Result = null
                });
            }

            try
            {
                var result = await _serviceProviders.AuthenticationService.Authenticate(request);
                var response = new ApiResponse<AuthenticationResponse>
                {
                    Code = 1000,
                    Message = "Authentication successful.",
                    Result = result
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<AuthenticationResponse>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };

                // Return appropriate HTTP status based on error code
                return ex.ErrorCode switch
                {
                    ErrorCode.EMAIL_NOT_EXIST or ErrorCode.UNAUTHENTICATED => Unauthorized(errorResponse),
                    _ => BadRequest(errorResponse)
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during authentication: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                var errorResponse = new ApiResponse<AuthenticationResponse>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred during authentication.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Introspect endpoint - Validate JWT token
        /// </summary>
        [HttpPost("introspect")]
        public async Task<IActionResult> Introspect([FromBody] IntrospectRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<IntrospectResponse>
                {
                    Code = 400,
                    Message = "Invalid introspect data.",
                    Result = null
                });
            }

            try
            {
                var result = await _serviceProviders.AuthenticationService.Introspect(request);
                var response = new ApiResponse<IntrospectResponse>
                {
                    Code = 1000,
                    Message = "Token introspection successful.",
                    Result = result
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<IntrospectResponse>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during token introspection: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                var errorResponse = new ApiResponse<IntrospectResponse>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred during token introspection.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Logout endpoint - Invalidate JWT token
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Code = 400,
                    Message = "Invalid logout data.",
                    Result = null
                });
            }

            try
            {
                await _serviceProviders.AuthenticationService.Logout(request);
                var response = new ApiResponse<object>
                {
                    Code = 1000,
                    Message = "Logout successful.",
                    Result = null
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<object>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during logout: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                var errorResponse = new ApiResponse<object>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred during logout.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }
    }
}
