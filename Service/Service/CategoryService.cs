using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Repository;
using Repository.Repository.Interface;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService() => _unitOfWork = new UnitOfWork();

        public CategoryService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        /// <summary>
        /// Create a new category
        /// Equivalent to Java: createCategory(CategoryRequest request)
        /// </summary>
        public async Task<CategoryResponse> CreateCategory(CategoryRequest request)
        {
            try
            {
                return await _unitOfWork.CategoryRepository.Create(request);
            }
            catch (InvalidOperationException ex)
            {
                // Category code already exists - equivalent to CATEGORY_CODE_EXIST in Java
                throw new InvalidOperationException($"Category code '{request.CategoryCode}' already exists.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the category.", ex);
            }
        }

        /// <summary>
        /// Get category by category code
        /// Equivalent to Java: getCategoryByCode(String categoryCode)
        /// </summary>
        public async Task<CategoryResponse?> GetCategoryByCode(string categoryCode)
        {
            try
            {
                var category = await _unitOfWork.CategoryRepository.GetByCode(categoryCode);

                if (category == null)
                {
                    // Equivalent to CATEGORY_NOT_FOUND in Java
                    throw new KeyNotFoundException($"Category with code '{categoryCode}' not found.");
                }

                return category;
            }
            catch (KeyNotFoundException)
            {
                throw; // Re-throw KeyNotFoundException as is
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the category.", ex);
            }
        }

        /// <summary>
        /// Get all categories
        /// Equivalent to Java: getAllCategories()
        /// </summary>
        public async Task<List<CategoryResponse>> GetAllCategories()
        {
            try
            {
                return await _unitOfWork.CategoryRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving categories.", ex);
            }
        }

        /// <summary>
        /// Update category by category code
        /// Equivalent to Java: updateCategory(String categoryCode, CategoryRequest request)
        /// </summary>
        public async Task<CategoryResponse> UpdateCategory(string categoryCode, CategoryRequest request)
        {
            try
            {
                return await _unitOfWork.CategoryRepository.Update(categoryCode, request);
            }
            catch (KeyNotFoundException ex)
            {
                // Category not found - equivalent to CATEGORY_NOT_FOUND in Java
                throw new KeyNotFoundException($"Category with code '{categoryCode}' not found.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the category.", ex);
            }
        }

        /// <summary>
        /// Delete category by category code
        /// Equivalent to Java: deleteCategory(String categoryCode)
        /// </summary>
        public async Task DeleteCategory(string categoryCode)
        {
            try
            {
                await _unitOfWork.CategoryRepository.Delete(categoryCode);
            }
            catch (KeyNotFoundException ex)
            {
                // Category not found - equivalent to CATEGORY_NOT_FOUND in Java
                throw new KeyNotFoundException($"Category with code '{categoryCode}' not found.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the category.", ex);
            }
        }
    }
}
