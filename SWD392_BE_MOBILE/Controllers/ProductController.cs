using Microsoft.AspNetCore.Mvc;
using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Models.Exceptions;
using Repository.Models.Enums;
using Service.Service.Interface;

namespace SWD392_BE_MOBILE.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IServiceProviders _serviceProviders;

        public ProductController(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<ProductResponse>
                {
                    Code = 400,
                    Message = "Invalid product data.",
                    Result = null
                });
            }

            try
            {
                var result = await _serviceProviders.ProductService.CreateProduct(request);
                var response = new ApiResponse<ProductResponse>
                {
                    Code = 1000,
                    Message = "Product created successfully.",
                    Result = result
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<ProductResponse>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating product: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                var errorResponse = new ApiResponse<ProductResponse>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred while creating the product.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Get product by code
        /// </summary>
        [HttpGet("{productCode}")]
        public async Task<IActionResult> GetProduct(string productCode)
        {
            try
            {
                var result = await _serviceProviders.ProductService.GetProductByCode(productCode);
                var response = new ApiResponse<ProductResponse>
                {
                    Code = 1000,
                    Message = "Product retrieved successfully.",
                    Result = result
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<ProductResponse>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting product: {ex.Message}");

                var errorResponse = new ApiResponse<ProductResponse>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred while retrieving the product.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Get all products
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var result = await _serviceProviders.ProductService.GetAllProducts();
                var response = new ApiResponse<List<ProductResponse>>
                {
                    Code = 1000,
                    Message = "Products retrieved successfully.",
                    Result = result
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<List<ProductResponse>>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting products: {ex.Message}");

                var errorResponse = new ApiResponse<List<ProductResponse>>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred while retrieving products.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Update product by code
        /// </summary>
        [HttpPut("{productCode}")]
        public async Task<IActionResult> UpdateProduct(string productCode, [FromBody] ProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<ProductResponse>
                {
                    Code = 400,
                    Message = "Invalid product data.",
                    Result = null
                });
            }

            try
            {
                var result = await _serviceProviders.ProductService.UpdateProduct(productCode, request);
                var response = new ApiResponse<ProductResponse>
                {
                    Code = 1000,
                    Message = "Product updated successfully.",
                    Result = result
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<ProductResponse>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product: {ex.Message}");

                var errorResponse = new ApiResponse<ProductResponse>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred while updating the product.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Mark product as out of stock
        /// </summary>
        [HttpPut("{productCode}/outofstock")]
        public async Task<IActionResult> MarkProductOutOfStock(string productCode)
        {
            try
            {
                await _serviceProviders.ProductService.DeleteProduct(productCode);
                var response = new ApiResponse<object>
                {
                    Code = 1000,
                    Message = "Product marked as out of stock successfully.",
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
                Console.WriteLine($"Error marking product out of stock: {ex.Message}");

                var errorResponse = new ApiResponse<object>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred while marking product out of stock.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }
    }
}
