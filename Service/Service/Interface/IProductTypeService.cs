using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;

namespace Service.Service.Interface
{
    public interface IProductTypeService
    {
        Task<ProductTypeResponse> CreateProductType(ProductTypeRequest request);
        Task<ProductTypeResponse> GetProductTypeByCode(string productTypeCode);
        Task<List<ProductTypeResponse>> GetAllProductTypes();
        Task<ProductTypeResponse> UpdateProductType(string productTypeCode, ProductTypeRequest request);
        Task DeleteProductType(string productTypeCode);
    }
}
