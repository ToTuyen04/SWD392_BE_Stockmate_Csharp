
using Microsoft.AspNetCore.Mvc;
using Repository.Data;
using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Service.Service.Interface;

namespace SWD392_BE_MOBILE.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IServiceProviders _serviceProviders;

        public CategoryController(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _serviceProviders.CategoryService.GetAllCategories();

                var response = new ApiResponse<List<CategoryResponse>>
                {
                    Code = 1000,
                    Message = null,
                    Result = categories
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<List<CategoryResponse>>
                {
                    Code = 9999,
                    Message = "An error occurred while retrieving categories.",
                    Result = null
                };

                return BadRequest(errorResponse);
            }
        }

        [HttpGet("{categoryCode}")]
        public async Task<IActionResult> GetCategoryByCode(string categoryCode)
        {
            try
            {
                var category = await _serviceProviders.CategoryService.GetCategoryByCode(categoryCode);

                var response = new ApiResponse<CategoryResponse>
                {
                    Code = 1000,
                    Message = null,
                    Result = category
                };
                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                var errorResponse = new ApiResponse<CategoryResponse>
                {
                    Code = 1036,
                    Message = "Category code is not found",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<CategoryResponse>
                {
                    Code = 9999,
                    Message = "An error occurred while retrieving the category.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryRequest category)
        {
            if (category == null || string.IsNullOrEmpty(category.CategoryCode) || string.IsNullOrEmpty(category.CategoryName))
            {
                return BadRequest(new ApiResponse<CategoryResponse>
                {
                    Code = 400,
                    Message = "Invalid category data.",
                    Result = null
                });
            }
            try
            {
                var createdCategory = await _serviceProviders.CategoryService.CreateCategory(category);
                var response = new ApiResponse<CategoryResponse>
                {
                    Code = 1000,
                    Message = "Category created successfully.",
                    Result = createdCategory
                };
                return CreatedAtAction(nameof(GetCategoryByCode), new { categoryCode = createdCategory.CategoryCode }, response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<CategoryResponse>
                {
                    Code = 9999,
                    Message = "An error occurred while creating the category.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        [HttpPut("{categoryCode}")]
        public async Task<IActionResult> UpdateCategory(string categoryCode, [FromBody] CategoryRequest category)
        {
            if (category == null || string.IsNullOrEmpty(category.CategoryName))
            {
                return BadRequest(new ApiResponse<CategoryResponse>
                {
                    Code = 400,
                    Message = "Invalid category data.",
                    Result = null
                });
            }
            try
            {
                var updatedCategory = await _serviceProviders.CategoryService.UpdateCategory(categoryCode, category);
                var response = new ApiResponse<CategoryResponse>
                {
                    Code = 1000,
                    Message = "Category updated successfully.",
                    Result = updatedCategory
                };
                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                var errorResponse = new ApiResponse<object>
                {
                    Code = 1036,
                    Message = "Category code is not found",
                    Result = null
                };

                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<CategoryResponse>
                {
                    Code = 9999,
                    Message = "An error occurred while updating the category.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        [HttpDelete("{categoryCode}")]
        public async Task<IActionResult> DeleteCategory(string categoryCode)
        {
            try
            {
                await _serviceProviders.CategoryService.DeleteCategory(categoryCode);

                var response = new ApiResponse<object>
                {
                    Code = 1000,
                    Message = "Category deleted successfully",
                    Result = null
                };

                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                var errorResponse = new ApiResponse<object>
                {
                    Code = 1036,
                    Message = "Category code is not found",
                    Result = null
                };

                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<object>
                {
                    Code = 9999,
                    Message = "An error occurred while deleting the category.",
                    Result = null
                };

                return BadRequest(errorResponse);
            }
        }
    }
}
