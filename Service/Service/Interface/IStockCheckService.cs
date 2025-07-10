using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;

namespace Service.Service.Interface
{
    /// <summary>
    /// Interface for StockCheck service that combines both stock check note and stock check product operations
    /// </summary>
    public interface IStockCheckService
    {
        #region Stock Check Note Methods

        /// <summary>
        /// Create new stock check note
        /// </summary>
        Task<StockCheckNoteResponse> CreateStockCheckNote(StockCheckNoteRequest request);

        /// <summary>
        /// Get all stock check notes
        /// </summary>
        Task<List<StockCheckNoteResponse>> GetAllStockCheckNotes();

        /// <summary>
        /// Get stock check notes by warehouse
        /// </summary>
        Task<List<StockCheckNoteResponse>> GetStockCheckNotesByWarehouse(string warehouseCode);

        /// <summary>
        /// Approve stock check note
        /// </summary>
        Task<StockCheckNoteResponse> ApproveStockCheck(string id);

        /// <summary>
        /// Finalize stock check note
        /// </summary>
        Task<StockCheckNoteResponse> FinalizeStockCheck(string id, bool isFinished);

        /// <summary>
        /// Get stock check notes by status
        /// </summary>
        Task<List<StockCheckNoteResponse>> GetStockCheckNotesByStatus(string status);

        #endregion

        #region Stock Check Product Methods

        /// <summary>
        /// Get stock check products for a specific note
        /// </summary>
        Task<List<StockCheckProductResponse>> GetStockCheckProducts(string stockCheckNoteId);

        /// <summary>
        /// Update stock check product quantity
        /// </summary>
        Task<StockCheckProductResponse> UpdateStockCheckProduct(string stockCheckNoteId, string productCode, StockCheckProductRequest request);

        /// <summary>
        /// Add product to stock check note
        /// </summary>
        Task<StockCheckProductResponse> AddProductToStockCheck(string stockCheckNoteId, StockCheckProductRequest request);

        /// <summary>
        /// Remove product from stock check note
        /// </summary>
        Task RemoveProductFromStockCheck(string stockCheckNoteId, string productCode);

        #endregion
    }
}
