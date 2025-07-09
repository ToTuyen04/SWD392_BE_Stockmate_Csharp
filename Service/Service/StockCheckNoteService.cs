using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Models.Exceptions;
using Repository.Models.Enums;
using Repository.Repository.Interface;
using Repository.Repository;
using Service.Service.Interface;

namespace Service.Service
{
    public class StockCheckNoteService : IStockCheckNoteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StockCheckNoteService() => _unitOfWork = new UnitOfWork();

        public async Task<StockCheckNoteResponse> CreateStockCheckNote(StockCheckNoteRequest request)
        {
            // TODO: Implement stock check note creation logic with proper repository methods
            throw new AppException(ErrorCode.UNKNOWN_ERROR, "StockCheckNote service not fully implemented yet");
        }

        public async Task<List<StockCheckNoteResponse>> GetAllStockCheckNotes()
        {
            // TODO: Implement get all stock check notes logic
            throw new AppException(ErrorCode.UNKNOWN_ERROR, "StockCheckNote service not fully implemented yet");
        }

        public async Task<List<StockCheckNoteResponse>> GetStockCheckNotesByWarehouse(string warehouseCode)
        {
            // TODO: Implement get stock check notes by warehouse logic
            throw new AppException(ErrorCode.WAREHOUSE_NOT_FOUND);
        }

        public async Task<StockCheckNoteResponse> ApproveStockCheck(string id)
        {
            // TODO: Implement approve stock check logic
            throw new AppException(ErrorCode.STOCK_CHECK_NOTE_NOT_FOUND);
        }

        public async Task<StockCheckNoteResponse> FinalizeStockCheck(string id, bool isFinished)
        {
            // TODO: Implement finalize stock check logic
            throw new AppException(ErrorCode.STOCK_CHECK_NOTE_NOT_FOUND);
        }

        public async Task<List<StockCheckNoteResponse>> GetStockCheckNotesByStatus(string status)
        {
            // TODO: Implement get stock check notes by status logic
            throw new AppException(ErrorCode.UNKNOWN_ERROR, "StockCheckNote service not fully implemented yet");
        }
    }
}
