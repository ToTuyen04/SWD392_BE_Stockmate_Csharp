using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) => _context = context;
        public async Task<List<CategoryResponse>> GetAll()
        {
            var categories = await _context.Categories
                .Select(c => new CategoryResponse
                {
                    CategoryCode = c.CategoryCode,
                    CategoryName = c.CategoryName
                })
                .ToListAsync();
            return categories;
        }

        public async Task<CategoryResponse?> GetByCode(string categoryCode)
        {
            var category = await _context.Categories
                .Where(c => c.CategoryCode == categoryCode)
                .Select(c => new CategoryResponse
                {
                    CategoryCode = c.CategoryCode,
                    CategoryName = c.CategoryName
                })
                .FirstOrDefaultAsync();
            return category;
        }

        public async Task<CategoryResponse> Create(CategoryRequest request)
        {
            // Check if category code already exists
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryCode == request.CategoryCode);

            if (existingCategory != null)
            {
                throw new InvalidOperationException($"Category with code '{request.CategoryCode}' already exists.");
            }

            var category = new Models.Entities.Category
            {
                CategoryId = Guid.NewGuid().ToString(),
                CategoryCode = request.CategoryCode,
                CategoryName = request.CategoryName
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new CategoryResponse
            {
                CategoryCode = category.CategoryCode,
                CategoryName = category.CategoryName
            };
        }

        public async Task<CategoryResponse> Update(string categoryCode, CategoryRequest request)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryCode == categoryCode);

            if (category == null)
            {
                throw new KeyNotFoundException($"Category with code '{categoryCode}' not found.");
            }

            // Update properties
            category.CategoryName = request.CategoryName;

            // Save changes
            await _context.SaveChangesAsync();

            return new CategoryResponse
            {
                CategoryCode = category.CategoryCode,
                CategoryName = category.CategoryName
            };
        }

        public async Task Delete(string categoryCode)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryCode == categoryCode);

            if (category == null)
            {
                throw new KeyNotFoundException($"Category with code '{categoryCode}' not found.");
            }

            // Remove the category
            _context.Categories.Remove(category);

            // Save changes
            await _context.SaveChangesAsync();
        }
    }
}
