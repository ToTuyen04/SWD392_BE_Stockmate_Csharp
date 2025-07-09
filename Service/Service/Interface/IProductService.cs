using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;

namespace Service.Service.Interface
{
    public interface IProductService
    {
        Task<ProductResponse> CreateProduct(ProductRequest request);
        Task<ProductResponse> GetProductByCode(string productCode);
        Task<List<ProductResponse>> GetAllProducts();
        Task<ProductResponse> UpdateProduct(string productCode, ProductRequest request);
        Task DeleteProduct(string productCode);
    }
}
