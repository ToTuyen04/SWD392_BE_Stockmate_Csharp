using Microsoft.AspNetCore.Mvc;
using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Models.Exceptions;
using Repository.Models.Enums;
using Service.Service.Interface;

namespace SWD392_BE_MOBILE.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class StockTransactionController : ControllerBase
    {
        private readonly IServiceProviders _serviceProviders;

        public StockTransactionController(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        /// <summary>
        /// Create a new stock transaction
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> CreateTransaction([FromBody] StockExchangeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<StockExchangeResponse>
                {
                    Code = 400,
                    Message = "Invalid transaction data.",
                    Result = null
                });
            }

            try
            {
                var result = await _serviceProviders.StockTransactionService.CreateTransaction(request);
                var response = new ApiResponse<StockExchangeResponse>
                {
                    Code = 1000,
                    Message = "Transaction created successfully.",
                    Result = result
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<StockExchangeResponse>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating transaction: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                var errorResponse = new ApiResponse<StockExchangeResponse>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred while creating the transaction.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Approve a transaction
        /// </summary>
        [HttpPost("approve/{exchangeNoteId}")]
        public async Task<IActionResult> ApproveTransaction(string exchangeNoteId)
        {
            try
            {
                var result = await _serviceProviders.StockTransactionService.ApproveTransaction(exchangeNoteId);
                var response = new ApiResponse<StockExchangeResponse>
                {
                    Code = 1000,
                    Message = "Transaction approved successfully.",
                    Result = result
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<StockExchangeResponse>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error approving transaction: {ex.Message}");

                var errorResponse = new ApiResponse<StockExchangeResponse>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred while approving the transaction.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Finalize a transaction
        /// </summary>
        [HttpPost("finalize/{exchangeNoteId}")]
        public async Task<IActionResult> FinalizeTransaction(string exchangeNoteId, [FromQuery] bool includeItems = false)
        {
            try
            {
                var result = await _serviceProviders.StockTransactionService.FinalizeTransaction(exchangeNoteId);
                var response = new ApiResponse<StockExchangeResponse>
                {
                    Code = 1000,
                    Message = "Transaction finalized successfully.",
                    Result = result
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<StockExchangeResponse>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error finalizing transaction: {ex.Message}");

                var errorResponse = new ApiResponse<StockExchangeResponse>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred while finalizing the transaction.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Cancel a transaction
        /// </summary>
        [HttpPost("cancel/{exchangeNoteId}")]
        public async Task<IActionResult> CancelTransaction(string exchangeNoteId)
        {
            try
            {
                var result = await _serviceProviders.StockTransactionService.CancelTransaction(exchangeNoteId);
                var response = new ApiResponse<StockExchangeResponse>
                {
                    Code = 1000,
                    Message = "Transaction cancelled successfully.",
                    Result = result
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<StockExchangeResponse>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cancelling transaction: {ex.Message}");

                var errorResponse = new ApiResponse<StockExchangeResponse>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred while cancelling the transaction.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Get all transactions
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllTransactions()
        {
            try
            {
                var result = await _serviceProviders.StockTransactionService.GetAllTransactions();
                var response = new ApiResponse<List<StockExchangeResponse>>
                {
                    Code = 1000,
                    Message = "Transactions retrieved successfully.",
                    Result = result
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<List<StockExchangeResponse>>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting transactions: {ex.Message}");

                var errorResponse = new ApiResponse<List<StockExchangeResponse>>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred while retrieving transactions.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Get transactions by warehouse
        /// </summary>
        [HttpGet("warehouse/{warehouseCode}")]
        public async Task<IActionResult> GetTransactionsByWarehouse(string warehouseCode)
        {
            try
            {
                var result = await _serviceProviders.StockTransactionService.GetTransactionsByWarehouse(warehouseCode);
                var response = new ApiResponse<List<StockExchangeResponse>>
                {
                    Code = 1000,
                    Message = "Transactions retrieved successfully.",
                    Result = result
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<List<StockExchangeResponse>>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting transactions by warehouse: {ex.Message}");

                var errorResponse = new ApiResponse<List<StockExchangeResponse>>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred while retrieving transactions by warehouse.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Get pending transactions
        /// </summary>
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingTransactions()
        {
            try
            {
                var result = await _serviceProviders.StockTransactionService.GetPendingTransactions();
                var response = new ApiResponse<List<StockExchangeResponse>>
                {
                    Code = 1000,
                    Message = "Pending transactions retrieved successfully.",
                    Result = result
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<List<StockExchangeResponse>>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting pending transactions: {ex.Message}");

                var errorResponse = new ApiResponse<List<StockExchangeResponse>>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred while retrieving pending transactions.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Get transaction by ID
        /// </summary>
        [HttpGet("{exchangeNoteId}")]
        public async Task<IActionResult> GetTransactionById(string exchangeNoteId)
        {
            try
            {
                var result = await _serviceProviders.StockTransactionService.GetTransactionById(exchangeNoteId);
                var response = new ApiResponse<StockExchangeResponse>
                {
                    Code = 1000,
                    Message = "Transaction retrieved successfully.",
                    Result = result
                };
                return Ok(response);
            }
            catch (AppException ex)
            {
                var errorResponse = new ApiResponse<StockExchangeResponse>
                {
                    Code = (int)ex.ErrorCode,
                    Message = ex.Message,
                    Result = null
                };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting transaction: {ex.Message}");

                var errorResponse = new ApiResponse<StockExchangeResponse>
                {
                    Code = (int)ErrorCode.UNCATEGORIZED_EXCEPTION,
                    Message = "An error occurred while retrieving the transaction.",
                    Result = null
                };
                return BadRequest(errorResponse);
            }
        }
    }
}
