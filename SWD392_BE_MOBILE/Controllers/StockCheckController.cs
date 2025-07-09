using Microsoft.AspNetCore.Mvc;
using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Models.Exceptions;
using Repository.Models.Enums;
using Service.Service.Interface;

namespace SWD392_BE_MOBILE.Controllers
{
    [ApiController]
    [Route("api/stock-check")]
    public class StockCheckController : ControllerBase
    {
        private readonly IServiceProviders _serviceProviders;

        public StockCheckController(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        /// <summary>
        /// Create a new stock check note
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> CreateStockCheckNote([FromBody] StockCheckNoteRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<StockCheckNoteResponse>
                {
                    Code = 400,
                    Message = "Invalid stock check note data.",
                    Result = null
                });
            }

            try
            {
                var result = await _serviceProviders.StockCheckNoteService.CreateStockCheckNote(request);
                var response = new ApiResponse<StockCheckNoteResponse>
                {
                    Code = 1000,
                    Message = "Stock check note created successfully.",
                    Result = result
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<StockCheckNoteResponse>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating stock check note: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                var errorResponse = new ApiResponse<StockCheckNoteResponse>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred while creating the stock check note.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Get all stock check notes
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllStockCheckNotes()
        {
            try
            {
                var result = await _serviceProviders.StockCheckNoteService.GetAllStockCheckNotes();
                var response = new ApiResponse<List<StockCheckNoteResponse>>
                {
                    Code = 1000,
                    Message = "Stock check notes retrieved successfully.",
                    Result = result
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<List<StockCheckNoteResponse>>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting stock check notes: {ex.Message}");

                var errorResponse = new ApiResponse<List<StockCheckNoteResponse>>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred while retrieving stock check notes.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Get stock check notes by warehouse
        /// </summary>
        [HttpGet("warehouse/{warehouseCode}")]
        public async Task<IActionResult> GetStockCheckNotesByWarehouse(string warehouseCode)
        {
            try
            {
                var result = await _serviceProviders.StockCheckNoteService.GetStockCheckNotesByWarehouse(warehouseCode);
                var response = new ApiResponse<List<StockCheckNoteResponse>>
                {
                    Code = 1000,
                    Message = "Stock check notes retrieved successfully.",
                    Result = result
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<List<StockCheckNoteResponse>>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting stock check notes by warehouse: {ex.Message}");

                var errorResponse = new ApiResponse<List<StockCheckNoteResponse>>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred while retrieving stock check notes by warehouse.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Approve stock check note
        /// </summary>
        [HttpPost("approve/{id}")]
        public async Task<IActionResult> ApproveStockCheck(string id)
        {
            try
            {
                var result = await _serviceProviders.StockCheckNoteService.ApproveStockCheck(id);
                var response = new ApiResponse<StockCheckNoteResponse>
                {
                    Code = 1000,
                    Message = "Stock check note approved successfully.",
                    Result = result
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<StockCheckNoteResponse>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error approving stock check: {ex.Message}");

                var errorResponse = new ApiResponse<StockCheckNoteResponse>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred while approving stock check.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Finalize stock check note
        /// </summary>
        [HttpPost("finalize/{id}")]
        public async Task<IActionResult> FinalizeStockCheck(string id, [FromQuery] bool isFinished)
        {
            try
            {
                var result = await _serviceProviders.StockCheckNoteService.FinalizeStockCheck(id, isFinished);
                var response = new ApiResponse<StockCheckNoteResponse>
                {
                    Code = 1000,
                    Message = "Stock check note finalized successfully.",
                    Result = result
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<StockCheckNoteResponse>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error finalizing stock check: {ex.Message}");

                var errorResponse = new ApiResponse<StockCheckNoteResponse>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred while finalizing stock check.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Get stock check notes by status
        /// </summary>
        [HttpGet("filter")]
        public async Task<IActionResult> GetStockCheckNotesByStatus([FromQuery] string? status)
        {
            try
            {
                List<StockCheckNoteResponse> result;

                if (string.IsNullOrWhiteSpace(status))
                {
                    result = await _serviceProviders.StockCheckNoteService.GetAllStockCheckNotes();
                }
                else
                {
                    result = await _serviceProviders.StockCheckNoteService.GetStockCheckNotesByStatus(status);
                }

                var response = new ApiResponse<List<StockCheckNoteResponse>>
                {
                    Code = 1000,
                    Message = "Stock check notes retrieved successfully.",
                    Result = result
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<List<StockCheckNoteResponse>>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting stock check notes by status: {ex.Message}");

                var errorResponse = new ApiResponse<List<StockCheckNoteResponse>>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred while retrieving stock check notes by status.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }
    }
}
