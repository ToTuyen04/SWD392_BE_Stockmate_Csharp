using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Models.Entities;
using Repository.Models.Enums;

namespace Repository.Repository.Interface
{
    public interface IStockCheckProductRepository
    {
        Task<List<StockCheckProductResponse>> GetByStockCheckNote(string stockCheckNoteId);
        Task<StockCheckProductResponse> UpdateActualQuantity(string stockCheckNoteId, string productCode, int actualQuantity);
        Task<StockCheckProductResponse> AddToStockCheck(string stockCheckNoteId, StockCheckProductRequest request);
        Task RemoveFromStockCheck(string stockCheckNoteId, string productCode);
        Task<StockCheckProduct> GetLatestStockCheck(string productCode, string warehouseCode);
        Task<List<StockCheckProduct>> GetByStockCheckNoteAndStatus(string stockCheckNoteId, StockCheckProductStatus status);
        Task UpdateProductsStatus(List<StockCheckProduct> products, StockCheckProductStatus status);
        Task DeleteProducts(List<StockCheckProduct> products);
    }
}
