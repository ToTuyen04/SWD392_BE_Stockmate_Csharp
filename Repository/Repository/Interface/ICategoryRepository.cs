using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.Interface
{
    public interface ICategoryRepository
    {
        Task<List<CategoryResponse>> GetAll();
        Task<CategoryResponse?> GetByCode(string categoryCode);
        Task<CategoryResponse> Create(CategoryRequest request);
        Task<CategoryResponse> Update(string categoryCode, CategoryRequest request);
        Task Delete(string categoryCode);
    }
}
