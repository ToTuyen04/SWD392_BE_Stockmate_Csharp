using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Models.Entities;
using Repository.Models.Enums;

namespace Repository.Repository.Interface
{
    public interface IStockCheckNoteRepository
    {
        Task<StockCheckNoteResponse> Create(StockCheckNoteRequest request, string checkerUserCode);
        Task<List<StockCheckNoteResponse>> GetAll();
        Task<List<StockCheckNoteResponse>> GetByWarehouse(string warehouseCode);
        Task<StockCheckNoteResponse> GetByCode(string stockCheckNoteId);
        Task<StockCheckNoteResponse> Update(string stockCheckNoteId, StockCheckStatus status);
        Task<List<StockCheckNoteResponse>> GetByStatus(StockCheckStatus status);
        Task<StockCheckNote> GetEntityByCode(string stockCheckNoteId);
    }
}
