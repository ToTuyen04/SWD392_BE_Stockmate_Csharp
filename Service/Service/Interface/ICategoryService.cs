using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service.Interface
{
    public interface ICategoryService
    {
        /// <summary>
        /// Get all categories
        /// </summary>
        Task<List<CategoryResponse>> GetAllCategories();

        /// <summary>
        /// Get category by category code
        /// </summary>
        Task<CategoryResponse?> GetCategoryByCode(string categoryCode);

        /// <summary>
        /// Create a new category
        /// </summary>
        Task<CategoryResponse> CreateCategory(CategoryRequest request);

        /// <summary>
        /// Update category by category code
        /// </summary>
        Task<CategoryResponse> UpdateCategory(string categoryCode, CategoryRequest request);

        /// <summary>
        /// Delete category by category code
        /// </summary>
        Task DeleteCategory(string categoryCode);
    }
}
