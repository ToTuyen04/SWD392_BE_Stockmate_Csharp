using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;

namespace Service.Service.Interface
{
    public interface IStockTransactionService
    {
        Task<StockExchangeResponse> CreateTransaction(StockExchangeRequest request);
        Task<StockExchangeResponse> ApproveTransaction(string exchangeNoteId);
        Task<StockExchangeResponse> FinalizeTransaction(string exchangeNoteId);
        Task<StockExchangeResponse> CancelTransaction(string exchangeNoteId);
        Task<List<StockExchangeResponse>> GetAllTransactions();
        Task<List<StockExchangeResponse>> GetTransactionsByWarehouse(string warehouseCode);
        Task<List<StockExchangeResponse>> GetPendingTransactions();
        Task<StockExchangeResponse> GetTransactionById(string exchangeNoteId);
    }
}
