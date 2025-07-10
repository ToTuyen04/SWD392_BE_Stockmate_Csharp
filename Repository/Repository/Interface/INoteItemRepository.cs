using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;

namespace Repository.Repository.Interface
{
    public interface INoteItemRepository
    {
        Task<int> GetTotalImportByProductAndWarehouse(string productCode, string warehouseCode);
        Task<int> GetTotalExportByProductAndWarehouse(string productCode, string warehouseCode);
        Task<int> GetTotalImportAfterLastCheck(string productCode, string warehouseCode, DateTime lastStockCheckDate);
        Task<int> GetTotalExportAfterLastCheck(string productCode, string warehouseCode, DateTime lastStockCheckDate);
        Task<int> GetTotalStockByProductAndWarehouse(string productCode, string warehouseCode);

        // Stock Transaction methods
        Task<NoteItemResponse> AddToTransaction(string exchangeNoteId, TransactionItemRequest request);
        Task<List<NoteItemResponse>> GetByTransactionId(string exchangeNoteId);
        Task<NoteItemResponse> UpdateQuantity(string noteItemCode, int quantity);
        Task<bool> Delete(string noteItemCode);
    }
}
