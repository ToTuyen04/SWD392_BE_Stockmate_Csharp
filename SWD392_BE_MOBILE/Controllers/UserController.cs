using Microsoft.AspNetCore.Mvc;
using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Service.Service.Interface;

namespace SWD392_BE_MOBILE.Controllers
{

    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IServiceProviders _serviceProviders;
        public UserController(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<UserResponse>
                {
                    Code = 400,
                    Message = "Invalid user data.",
                    Result = null
                });
            }

            if (request == null)
            {
                return BadRequest(new ApiResponse<UserResponse>
                {
                    Code = 400,
                    Message = "User request is required.",
                    Result = null
                });
            }

            try
            {
                var createdUser = await _serviceProviders.UserService.CreateUser(request);
                var response = new ApiResponse<UserResponse>
                {
                    Code = 1000,
                    Message = "User created successfully.",
                    Result = createdUser
                };
                return CreatedAtAction(nameof(GetUserByCode), new { userCode = createdUser.UserCode }, response);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("User with code") ||
                                                       ex.Message.Contains("Username") ||
                                                       ex.Message.Contains("Email"))
            {
                // Handle specific user validation errors
                int errorCode = 1001; // Default to USER_EXIST
                if (ex.Message.Contains("User with code"))
                    errorCode = 1018; // USER_CODE_EXIST
                else if (ex.Message.Contains("Email"))
                    errorCode = 1002; // EMAIL_EXIST

                var errorResponse = new ApiResponse<UserResponse>
                {
                    Code = errorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (KeyNotFoundException ex)
            {
                // Handle role or warehouse not found
                int errorCode = ex.Message.Contains("Role") ? 1009 : 1013; // ROLE_NOT_FOUND : WAREHOUSE_NOT_FOUND

                var errorResponse = new ApiResponse<UserResponse>
                {
                    Code = errorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                // Log the actual exception for debugging
                Console.WriteLine($"Error creating user: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                var errorResponse = new ApiResponse<UserResponse>
                {
                    Code = 9999,
                    Message = $"An error occurred while creating the user: {ex.Message}",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        [HttpGet("{userCode}")]
        public async Task<IActionResult> GetUserByCode(string userCode)
        {
            // Placeholder method for CreatedAtAction reference
            return Ok(new ApiResponse<UserResponse>
            {
                Code = 1000,
                Message = "Method not implemented yet.",
                Result = null
            });
        }

    }
}
