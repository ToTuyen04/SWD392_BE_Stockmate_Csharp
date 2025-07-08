using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Models.Enums;
using Repository.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) => _context = context;

        public async Task<List<ProductResponse>> GetAll()
        {
            var products = await _context.Products
                .Include(p => p.ProductType)
                .Select(p => new ProductResponse
                {
                    ProductCode = p.ProductCode,
                    ProductName = p.ProductName,
                    Size = p.Size,
                    Color = p.Color,
                    Quantity = p.Quantity,
                    ProductTypeName = p.ProductType.ProductTypeName
                })
                .ToListAsync();
            return products;
        }

        public async Task<ProductResponse?> GetByCode(string productCode)
        {
            var product = await _context.Products
                .Include(p => p.ProductType)
                .Where(p => p.ProductCode == productCode)
                .Select(p => new ProductResponse
                {
                    ProductCode = p.ProductCode,
                    ProductName = p.ProductName,
                    Size = p.Size,
                    Color = p.Color,
                    Quantity = p.Quantity,
                    ProductTypeName = p.ProductType.ProductTypeName
                })
                .FirstOrDefaultAsync();
            return product;
        }

        public async Task<ProductResponse> Create(ProductRequest request)
        {
            // Check if product code already exists
            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductCode == request.ProductCode);

            if (existingProduct != null)
            {
                throw new InvalidOperationException($"Product with code '{request.ProductCode}' already exists.");
            }

            // Find ProductType by ProductTypeCode
            var productType = await _context.ProductTypes
                .FirstOrDefaultAsync(pt => pt.ProductTypeCode == request.ProductTypeCode);

            if (productType == null)
            {
                throw new KeyNotFoundException($"ProductType with code '{request.ProductTypeCode}' not found.");
            }

            var product = new Models.Entities.Product
            {
                ProductId = Guid.NewGuid().ToString(),
                ProductCode = request.ProductCode,
                ProductName = request.ProductName,
                Size = request.Size,
                Color = request.Color,
                Quantity = request.Quantity,
                Status = ProductStatus.instock,
                ProductType = productType
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductResponse
            {
                ProductCode = product.ProductCode,
                ProductName = product.ProductName,
                Size = product.Size,
                Color = product.Color,
                Quantity = product.Quantity,
                ProductTypeName = productType.ProductTypeName
            };
        }

        public async Task<ProductResponse> Update(string productCode, ProductRequest request)
        {
            var product = await _context.Products
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(p => p.ProductCode == productCode);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with code '{productCode}' not found.");
            }

            // Update properties (không update ProductCode và ProductType)
            product.ProductName = request.ProductName;
            product.Size = request.Size;
            product.Color = request.Color;
            product.Quantity = request.Quantity;

            await _context.SaveChangesAsync();

            return new ProductResponse
            {
                ProductCode = product.ProductCode,
                ProductName = product.ProductName,
                Size = product.Size,
                Color = product.Color,
                Quantity = product.Quantity,
                ProductTypeName = product.ProductType.ProductTypeName
            };
        }

        public async Task Delete(string productCode)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductCode == productCode);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with code '{productCode}' not found.");
            }

            // Soft delete: mark as out of stock instead of hard delete
            product.Status = ProductStatus.outofstock;

            await _context.SaveChangesAsync();
        }
    }
}
