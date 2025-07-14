using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Models.Exceptions;
using Repository.Models.Enums;
using Repository.Repository.Interface;
using Repository.Repository;
using Service.Service.Interface;

namespace Service.Service
{
    /// <summary>
    /// StockCheck service that handles stock check note and stock check product operations
    /// Replaces both StockCheckNoteService and StockCheckProductService
    /// </summary>
    public class StockCheckService : IStockCheckService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StockCheckService() => _unitOfWork = new UnitOfWork();

        #region Stock Check Note Methods

        /// <summary>
        /// Create new stock check note
        /// </summary>
        public async Task<StockCheckNoteResponse> CreateStockCheckNote(StockCheckNoteRequest request)
        {
            try
            {
                // Check if warehouse exists first
                var warehouse = await _unitOfWork.WarehouseRepository.GetByCode(request.WarehouseCode);
                if (warehouse == null)
                {
                    throw new AppException(ErrorCode.WAREHOUSE_NOT_FOUND, $"Warehouse '{request.WarehouseCode}' not found");
                }

                // Check if products exist
                foreach (var productRequest in request.StockCheckProducts)
                {
                    var product = await _unitOfWork.ProductRepository.GetEntityByCode(productRequest.ProductCode);
                    if (product == null)
                    {
                        throw new AppException(ErrorCode.PRODUCT_NOT_FOUND, $"Product '{productRequest.ProductCode}' not found");
                    }
                }

                // For now, use a default checker user code - in real implementation, get from authentication context
                var checkerUserCode = "MA0001"; // TODO: Get from authentication context

                // Create stock check note
                var stockCheckNote = await _unitOfWork.StockCheckNoteRepository.Create(request, checkerUserCode);

                // Create stock check products
                foreach (var productRequest in request.StockCheckProducts)
                {
                    await _unitOfWork.StockCheckProductRepository.AddToStockCheck(stockCheckNote.StockCheckNoteId, productRequest);
                }

                // Get updated stock check note with products
                return await _unitOfWork.StockCheckNoteRepository.GetByCode(stockCheckNote.StockCheckNoteId);
            }
            catch (AppException)
            {
                throw; // Re-throw AppException as is
            }
            catch (Exception ex)
            {
                // Log the full exception details for debugging
                var innerMessage = ex.InnerException?.Message ?? "No inner exception";
                var fullMessage = $"Error creating stock check note: {ex.Message}. Inner: {innerMessage}";
                throw new AppException(ErrorCode.UNKNOWN_ERROR, fullMessage);
            }
        }

        /// <summary>
        /// Get all stock check notes
        /// </summary>
        public async Task<List<StockCheckNoteResponse>> GetAllStockCheckNotes()
        {
            return await _unitOfWork.StockCheckNoteRepository.GetAll();
        }

        /// <summary>
        /// Get stock check notes by warehouse
        /// </summary>
        public async Task<List<StockCheckNoteResponse>> GetStockCheckNotesByWarehouse(string warehouseCode)
        {
            try
            {
                return await _unitOfWork.StockCheckNoteRepository.GetByWarehouse(warehouseCode);
            }
            catch (Exception)
            {
                throw new AppException(ErrorCode.WAREHOUSE_NOT_FOUND);
            }
        }

        /// <summary>
        /// Approve stock check note
        /// </summary>
        public async Task<StockCheckNoteResponse> ApproveStockCheck(string id)
        {
            try
            {
                // Validate stock check note exists and is in pending status
                var stockCheckNote = await _unitOfWork.StockCheckNoteRepository.GetEntityByCode(id);

                if (stockCheckNote.StockCheckStatus != StockCheckStatus.pending)
                {
                    throw new AppException(ErrorCode.STOCK_CHECK_CANNOT_BE_MODIFIED);
                }

                // Update status to accepted
                return await _unitOfWork.StockCheckNoteRepository.Update(id, StockCheckStatus.accepted);
            }
            catch (KeyNotFoundException)
            {
                throw new AppException(ErrorCode.STOCK_CHECK_NOTE_NOT_FOUND);
            }
        }

        /// <summary>
        /// Finalize stock check note
        /// </summary>
        public async Task<StockCheckNoteResponse> FinalizeStockCheck(string id, bool isFinished)
        {
            try
            {
                // Validate stock check note exists and is in accepted status
                var stockCheckNote = await _unitOfWork.StockCheckNoteRepository.GetEntityByCode(id);

                if (stockCheckNote.StockCheckStatus != StockCheckStatus.accepted)
                {
                    throw new AppException(ErrorCode.STOCK_CHECK_CANNOT_BE_FINALIZED);
                }

                // Get temporary stock check products
                var stockCheckProducts = await _unitOfWork.StockCheckProductRepository
                    .GetByStockCheckNoteAndStatus(id, StockCheckProductStatus.temporary);

                if (stockCheckProducts.Count == 0)
                {
                    throw new AppException(ErrorCode.STOCK_CHECK_PRODUCTS_NOT_FOUND);
                }

                if (isFinished)
                {
                    // Mark products as finished and update stock check status
                    await _unitOfWork.StockCheckProductRepository.UpdateProductsStatus(stockCheckProducts, StockCheckProductStatus.finished);
                    return await _unitOfWork.StockCheckNoteRepository.Update(id, StockCheckStatus.finished);
                }
                else
                {
                    // Reject stock check and delete products
                    await _unitOfWork.StockCheckProductRepository.DeleteProducts(stockCheckProducts);
                    return await _unitOfWork.StockCheckNoteRepository.Update(id, StockCheckStatus.rejected);
                }
            }
            catch (KeyNotFoundException)
            {
                throw new AppException(ErrorCode.STOCK_CHECK_NOTE_NOT_FOUND);
            }
        }

        /// <summary>
        /// Get stock check notes by status
        /// </summary>
        public async Task<List<StockCheckNoteResponse>> GetStockCheckNotesByStatus(string status)
        {
            try
            {
                if (!Enum.TryParse<StockCheckStatus>(status, true, out var stockCheckStatus))
                {
                    throw new AppException(ErrorCode.INVALID_STATUS);
                }

                return await _unitOfWork.StockCheckNoteRepository.GetByStatus(stockCheckStatus);
            }
            catch (Exception ex) when (!(ex is AppException))
            {
                throw new AppException(ErrorCode.UNKNOWN_ERROR, ex.Message);
            }
        }

        #endregion

        #region Stock Check Product Methods

        /// <summary>
        /// Get stock check products for a specific note
        /// </summary>
        public async Task<List<StockCheckProductResponse>> GetStockCheckProducts(string stockCheckNoteId)
        {
            try
            {
                return await _unitOfWork.StockCheckProductRepository.GetByStockCheckNote(stockCheckNoteId);
            }
            catch (Exception)
            {
                throw new AppException(ErrorCode.STOCK_CHECK_NOTE_NOT_FOUND);
            }
        }

        /// <summary>
        /// Update stock check product quantity
        /// </summary>
        public async Task<StockCheckProductResponse> UpdateStockCheckProduct(string stockCheckNoteId, string productCode, StockCheckProductRequest request)
        {
            try
            {
                return await _unitOfWork.StockCheckProductRepository.UpdateActualQuantity(stockCheckNoteId, productCode, request.ActualQuantity);
            }
            catch (KeyNotFoundException)
            {
                throw new AppException(ErrorCode.STOCK_CHECK_PRODUCTS_NOT_FOUND);
            }
        }

        /// <summary>
        /// Add product to stock check note
        /// </summary>
        public async Task<StockCheckProductResponse> AddProductToStockCheck(string stockCheckNoteId, StockCheckProductRequest request)
        {
            try
            {
                return await _unitOfWork.StockCheckProductRepository.AddToStockCheck(stockCheckNoteId, request);
            }
            catch (KeyNotFoundException ex)
            {
                if (ex.Message.Contains("Stock check note"))
                    throw new AppException(ErrorCode.STOCK_CHECK_NOTE_NOT_FOUND);
                if (ex.Message.Contains("Product"))
                    throw new AppException(ErrorCode.PRODUCT_NOT_FOUND);
                throw new AppException(ErrorCode.UNKNOWN_ERROR, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                throw new AppException(ErrorCode.UNKNOWN_ERROR, ex.Message);
            }
        }

        /// <summary>
        /// Remove product from stock check note
        /// </summary>
        public async Task RemoveProductFromStockCheck(string stockCheckNoteId, string productCode)
        {
            try
            {
                await _unitOfWork.StockCheckProductRepository.RemoveFromStockCheck(stockCheckNoteId, productCode);
            }
            catch (KeyNotFoundException)
            {
                throw new AppException(ErrorCode.STOCK_CHECK_PRODUCTS_NOT_FOUND);
            }
        }

        #endregion
    }
}