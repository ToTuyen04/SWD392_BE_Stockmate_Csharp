using Microsoft.AspNetCore.Mvc;
using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Models.Exceptions;
using Repository.Models.Enums;
using Service.Service.Interface;

namespace SWD392_BE_MOBILE.Controllers
{
    [ApiController]
    [Route("api/product-types")]
    public class ProductTypeController : ControllerBase
    {
        private readonly IServiceProviders _serviceProviders;

        public ProductTypeController(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        /// <summary>
        /// Create a new product type
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateProductType([FromBody] ProductTypeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<ProductTypeResponse>
                {
                    Code = 400,
                    Message = "Invalid product type data.",
                    Result = null
                });
            }

            try
            {
                var result = await _serviceProviders.ProductTypeService.CreateProductType(request);
                var response = new ApiResponse<ProductTypeResponse>
                {
                    Code = 1000,
                    Message = "Product type created successfully.",
                    Result = result
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<ProductTypeResponse>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating product type: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                var errorResponse = new ApiResponse<ProductTypeResponse>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred while creating the product type.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Get product type by code
        /// </summary>
        [HttpGet("{productTypeCode}")]
        public async Task<IActionResult> GetProductType(string productTypeCode)
        {
            try
            {
                var result = await _serviceProviders.ProductTypeService.GetProductTypeByCode(productTypeCode);
                var response = new ApiResponse<ProductTypeResponse>
                {
                    Code = 1000,
                    Message = "Product type retrieved successfully.",
                    Result = result
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<ProductTypeResponse>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting product type: {ex.Message}");

                var errorResponse = new ApiResponse<ProductTypeResponse>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred while retrieving the product type.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Get all product types
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllProductTypes()
        {
            try
            {
                var result = await _serviceProviders.ProductTypeService.GetAllProductTypes();
                var response = new ApiResponse<List<ProductTypeResponse>>
                {
                    Code = 1000,
                    Message = "Product types retrieved successfully.",
                    Result = result
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<List<ProductTypeResponse>>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting product types: {ex.Message}");

                var errorResponse = new ApiResponse<List<ProductTypeResponse>>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred while retrieving product types.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Update product type by code
        /// </summary>
        [HttpPut("{productTypeCode}")]
        public async Task<IActionResult> UpdateProductType(string productTypeCode, [FromBody] ProductTypeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<ProductTypeResponse>
                {
                    Code = 400,
                    Message = "Invalid product type data.",
                    Result = null
                });
            }

            try
            {
                var result = await _serviceProviders.ProductTypeService.UpdateProductType(productTypeCode, request);
                var response = new ApiResponse<ProductTypeResponse>
                {
                    Code = 1000,
                    Message = "Product type updated successfully.",
                    Result = result
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<ProductTypeResponse>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product type: {ex.Message}");

                var errorResponse = new ApiResponse<ProductTypeResponse>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred while updating the product type.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Delete product type by code
        /// </summary>
        [HttpDelete("{productTypeCode}")]
        public async Task<IActionResult> DeleteProductType(string productTypeCode)
        {
            try
            {
                await _serviceProviders.ProductTypeService.DeleteProductType(productTypeCode);
                var response = new ApiResponse<object>
                {
                    Code = 1000,
                    Message = "Product type deleted successfully.",
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
                Console.WriteLine($"Error deleting product type: {ex.Message}");

                var errorResponse = new ApiResponse<object>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred while deleting the product type.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }
    }
}
