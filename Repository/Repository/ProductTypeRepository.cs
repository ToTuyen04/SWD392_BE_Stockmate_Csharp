using Repository.Data;
using Repository.Repository.Interface;
using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repository
{
    public class ProductTypeRepository : IProductTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductTypeRepository(ApplicationDbContext context) => _context = context;

        public async Task<ProductTypeResponse> Create(ProductTypeRequest request)
        {
            // Check if ProductType code already exists
            var existingProductType = await _context.ProductTypes
                .FirstOrDefaultAsync(pt => pt.ProductTypeCode == request.ProductTypeCode);

            if (existingProductType != null)
            {
                throw new InvalidOperationException($"ProductType with code '{request.ProductTypeCode}' already exists.");
            }

            // Check if Category exists
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryCode == request.CategoryCode);

            if (category == null)
            {
                throw new KeyNotFoundException($"Category with code '{request.CategoryCode}' not found.");
            }

            // Create new ProductType
            var productType = new ProductType
            {
                ProductTypeId = Guid.NewGuid().ToString(),
                ProductTypeCode = request.ProductTypeCode,
                ProductTypeName = request.ProductTypeName,
                Price = request.Price,
                CategoryCode = request.CategoryCode
            };

            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();

            return new ProductTypeResponse
            {
                ProductTypeCode = productType.ProductTypeCode,
                ProductTypeName = productType.ProductTypeName,
                Price = productType.Price ?? 0,
                CategoryName = category.CategoryName
            };
        }

        public async Task<ProductTypeResponse> GetByCode(string productTypeCode)
        {
            var productType = await _context.ProductTypes
                .Include(pt => pt.Category)
                .FirstOrDefaultAsync(pt => pt.ProductTypeCode == productTypeCode);

            if (productType == null)
            {
                throw new KeyNotFoundException($"ProductType with code '{productTypeCode}' not found.");
            }

            return new ProductTypeResponse
            {
                ProductTypeCode = productType.ProductTypeCode,
                ProductTypeName = productType.ProductTypeName,
                Price = productType.Price ?? 0,
                CategoryName = productType.Category.CategoryName
            };
        }

        public async Task<List<ProductTypeResponse>> GetAll()
        {
            var productTypes = await _context.ProductTypes
                .Include(pt => pt.Category)
                .ToListAsync();

            return productTypes.Select(pt => new ProductTypeResponse
            {
                ProductTypeCode = pt.ProductTypeCode,
                ProductTypeName = pt.ProductTypeName,
                Price = pt.Price ?? 0,
                CategoryName = pt.Category.CategoryName
            }).ToList();
        }

        public async Task<ProductTypeResponse> Update(string productTypeCode, ProductTypeRequest request)
        {
            var productType = await _context.ProductTypes
                .Include(pt => pt.Category)
                .FirstOrDefaultAsync(pt => pt.ProductTypeCode == productTypeCode);

            if (productType == null)
            {
                throw new KeyNotFoundException($"ProductType with code '{productTypeCode}' not found.");
            }

            // Check if new CategoryCode exists (if it's being changed)
            if (productType.CategoryCode != request.CategoryCode)
            {
                var newCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.CategoryCode == request.CategoryCode);

                if (newCategory == null)
                {
                    throw new KeyNotFoundException($"Category with code '{request.CategoryCode}' not found.");
                }
            }

            // Update properties (không update ProductTypeCode theo business logic)
            productType.ProductTypeName = request.ProductTypeName;
            productType.Price = request.Price;
            productType.CategoryCode = request.CategoryCode; // Allow CategoryCode update

            await _context.SaveChangesAsync();

            // Reload to get updated Category information
            await _context.Entry(productType).Reference(pt => pt.Category).LoadAsync();

            return new ProductTypeResponse
            {
                ProductTypeCode = productType.ProductTypeCode,
                ProductTypeName = productType.ProductTypeName,
                Price = productType.Price ?? 0,
                CategoryName = productType.Category.CategoryName
            };
        }

        public async Task Delete(string productTypeCode)
        {
            var productType = await _context.ProductTypes
                .FirstOrDefaultAsync(pt => pt.ProductTypeCode == productTypeCode);

            if (productType == null)
            {
                throw new KeyNotFoundException($"ProductType with code '{productTypeCode}' not found.");
            }

            _context.ProductTypes.Remove(productType);
            await _context.SaveChangesAsync();
        }

        public async Task<ProductType> GetEntityByCode(string productTypeCode)
        {
            var productType = await _context.ProductTypes
                .Include(pt => pt.Category)
                .FirstOrDefaultAsync(pt => pt.ProductTypeCode == productTypeCode);

            return productType;
        }
    }
}
