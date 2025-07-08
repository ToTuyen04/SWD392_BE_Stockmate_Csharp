using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.Interface
{
    public interface IProductRepository
    {
        Task<List<ProductResponse>> GetAll();
        Task<ProductResponse?> GetByCode(string productCode);
        Task<ProductResponse> Create(ProductRequest request);
        Task<ProductResponse> Update(string productCode, ProductRequest request);
        Task Delete(string productCode);
    }
}
