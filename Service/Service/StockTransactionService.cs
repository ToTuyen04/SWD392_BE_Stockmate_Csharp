using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Models.Exceptions;
using Repository.Models.Enums;
using Repository.Repository.Interface;
using Repository.Repository;
using Service.Service.Interface;

namespace Service.Service
{
    public class StockTransactionService : IStockTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StockTransactionService() => _unitOfWork = new UnitOfWork();

        public async Task<StockExchangeResponse> CreateTransaction(StockExchangeRequest request)
        {
            try
            {
                // Validate warehouses exist
                if (!string.IsNullOrEmpty(request.SourceWarehouseCode))
                {
                    var sourceWarehouse = await _unitOfWork.WarehouseRepository.GetByCode(request.SourceWarehouseCode);
                    if (sourceWarehouse == null)
                        throw new AppException(ErrorCode.WAREHOUSE_NOT_FOUND, $"Source warehouse '{request.SourceWarehouseCode}' not found");
                }

                if (!string.IsNullOrEmpty(request.DestinationWarehouseCode))
                {
                    var destinationWarehouse = await _unitOfWork.WarehouseRepository.GetByCode(request.DestinationWarehouseCode);
                    if (destinationWarehouse == null)
                        throw new AppException(ErrorCode.WAREHOUSE_NOT_FOUND, $"Destination warehouse '{request.DestinationWarehouseCode}' not found");
                }

                // Validate products exist
                foreach (var item in request.Items)
                {
                    var product = await _unitOfWork.ProductRepository.GetEntityByCode(item.ProductCode);
                    if (product == null)
                        throw new AppException(ErrorCode.PRODUCT_NOT_FOUND, $"Product '{item.ProductCode}' not found");
                }

                // Create transaction
                var transaction = await _unitOfWork.ExchangeNoteRepository.Create(request);

                // Add items to transaction
                foreach (var item in request.Items)
                {
                    await _unitOfWork.NoteItemRepository.AddToTransaction(transaction.TransactionId, item);
                }

                // Get complete transaction with items
                return await _unitOfWork.ExchangeNoteRepository.GetByCode(transaction.TransactionId);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AppException(ErrorCode.UNKNOWN_ERROR, $"Error creating transaction: {ex.Message}");
            }
        }

        public async Task<StockExchangeResponse> ApproveTransaction(string exchangeNoteId)
        {
            try
            {
                var transaction = await _unitOfWork.ExchangeNoteRepository.GetByCode(exchangeNoteId);
                if (transaction == null)
                    throw new AppException(ErrorCode.TRANSACTION_NOT_FOUND, $"Transaction '{exchangeNoteId}' not found");

                if (transaction.Status != StockExchangeStatus.pending)
                    throw new AppException(ErrorCode.INVALID_OPERATION, "Only pending transactions can be approved");

                // Use default approver for now
                var approvedTransaction = await _unitOfWork.ExchangeNoteRepository.Approve(exchangeNoteId, "USR001");
                return approvedTransaction ?? throw new AppException(ErrorCode.TRANSACTION_NOT_FOUND);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AppException(ErrorCode.UNKNOWN_ERROR, $"Error approving transaction: {ex.Message}");
            }
        }

        public async Task<StockExchangeResponse> FinalizeTransaction(string exchangeNoteId)
        {
            try
            {
                var transaction = await _unitOfWork.ExchangeNoteRepository.GetByCode(exchangeNoteId);
                if (transaction == null)
                    throw new AppException(ErrorCode.TRANSACTION_NOT_FOUND, $"Transaction '{exchangeNoteId}' not found");

                if (transaction.Status != StockExchangeStatus.accepted)
                    throw new AppException(ErrorCode.INVALID_OPERATION, "Only accepted transactions can be finalized");

                var finalizedTransaction = await _unitOfWork.ExchangeNoteRepository.UpdateStatus(exchangeNoteId, StockExchangeStatus.finished);
                return finalizedTransaction ?? throw new AppException(ErrorCode.TRANSACTION_NOT_FOUND);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AppException(ErrorCode.UNKNOWN_ERROR, $"Error finalizing transaction: {ex.Message}");
            }
        }

        public async Task<StockExchangeResponse> CancelTransaction(string exchangeNoteId)
        {
            try
            {
                var transaction = await _unitOfWork.ExchangeNoteRepository.GetByCode(exchangeNoteId);
                if (transaction == null)
                    throw new AppException(ErrorCode.TRANSACTION_NOT_FOUND, $"Transaction '{exchangeNoteId}' not found");

                if (transaction.Status == StockExchangeStatus.finished)
                    throw new AppException(ErrorCode.INVALID_OPERATION, "Finished transactions cannot be cancelled");

                var cancelledTransaction = await _unitOfWork.ExchangeNoteRepository.UpdateStatus(exchangeNoteId, StockExchangeStatus.rejected);
                return cancelledTransaction ?? throw new AppException(ErrorCode.TRANSACTION_NOT_FOUND);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AppException(ErrorCode.UNKNOWN_ERROR, $"Error cancelling transaction: {ex.Message}");
            }
        }

        public async Task<List<StockExchangeResponse>> GetAllTransactions()
        {
            try
            {
                return await _unitOfWork.ExchangeNoteRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new AppException(ErrorCode.UNKNOWN_ERROR, $"Error getting transactions: {ex.Message}");
            }
        }

        public async Task<List<StockExchangeResponse>> GetTransactionsByWarehouse(string warehouseCode)
        {
            try
            {
                // Validate warehouse exists
                var warehouse = await _unitOfWork.WarehouseRepository.GetByCode(warehouseCode);
                if (warehouse == null)
                    throw new AppException(ErrorCode.WAREHOUSE_NOT_FOUND, $"Warehouse '{warehouseCode}' not found");

                return await _unitOfWork.ExchangeNoteRepository.GetByWarehouse(warehouseCode);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AppException(ErrorCode.UNKNOWN_ERROR, $"Error getting transactions by warehouse: {ex.Message}");
            }
        }

        public async Task<List<StockExchangeResponse>> GetPendingTransactions()
        {
            try
            {
                return await _unitOfWork.ExchangeNoteRepository.GetByStatus(StockExchangeStatus.pending);
            }
            catch (Exception ex)
            {
                throw new AppException(ErrorCode.UNKNOWN_ERROR, $"Error getting pending transactions: {ex.Message}");
            }
        }

        public async Task<StockExchangeResponse> GetTransactionById(string exchangeNoteId)
        {
            try
            {
                var transaction = await _unitOfWork.ExchangeNoteRepository.GetByCode(exchangeNoteId);
                if (transaction == null)
                    throw new AppException(ErrorCode.TRANSACTION_NOT_FOUND, $"Transaction '{exchangeNoteId}' not found");

                return transaction;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AppException(ErrorCode.UNKNOWN_ERROR, $"Error getting transaction: {ex.Message}");
            }
        }
    }
}
