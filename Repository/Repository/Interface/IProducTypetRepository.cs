using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Models.Entities;

namespace Repository.Repository.Interface
{
    public interface IProductTypeRepository
    {
        Task<ProductTypeResponse> Create(ProductTypeRequest request);
        Task<ProductTypeResponse> GetByCode(string productTypeCode);
        Task<List<ProductTypeResponse>> GetAll();
        Task<ProductTypeResponse> Update(string productTypeCode, ProductTypeRequest request);
        Task Delete(string productTypeCode);
        Task<ProductType> GetEntityByCode(string productTypeCode);
    }
}
