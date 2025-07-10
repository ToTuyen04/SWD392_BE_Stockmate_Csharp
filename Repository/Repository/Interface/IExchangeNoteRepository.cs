using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Models.Entities;
using Repository.Models.Enums;

namespace Repository.Repository.Interface
{
    public interface IExchangeNoteRepository
    {
        Task<StockExchangeResponse> Create(StockExchangeRequest request);
        Task<StockExchangeResponse> GetByCode(string exchangeNoteId);
        Task<List<StockExchangeResponse>> GetAll();
        Task<List<StockExchangeResponse>> GetByWarehouse(string warehouseCode);
        Task<List<StockExchangeResponse>> GetByStatus(StockExchangeStatus status);
        Task<StockExchangeResponse> UpdateStatus(string exchangeNoteId, StockExchangeStatus status);
        Task<StockExchangeResponse> Approve(string exchangeNoteId, string approvedBy);
        Task<bool> Delete(string exchangeNoteId);
    }
}
