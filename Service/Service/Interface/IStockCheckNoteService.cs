using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;

namespace Service.Service.Interface
{
    public interface IStockCheckNoteService
    {
        Task<StockCheckNoteResponse> CreateStockCheckNote(StockCheckNoteRequest request);
        Task<List<StockCheckNoteResponse>> GetAllStockCheckNotes();
        Task<List<StockCheckNoteResponse>> GetStockCheckNotesByWarehouse(string warehouseCode);
        Task<StockCheckNoteResponse> ApproveStockCheck(string id);
        Task<StockCheckNoteResponse> FinalizeStockCheck(string id, bool isFinished);
        Task<List<StockCheckNoteResponse>> GetStockCheckNotesByStatus(string status);
    }
}
