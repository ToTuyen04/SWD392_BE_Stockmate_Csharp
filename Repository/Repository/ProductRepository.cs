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
                    ProductTypeName = p.ProductType.ProductTypeName,
                    Status = p.Status
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
                    ProductTypeName = p.ProductType.ProductTypeName,
                    Status = p.Status
                })
                .FirstOrDefaultAsync();
            return product;
        }

        public async Task<Models.Entities.Product?> GetEntityByCode(string productCode)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.ProductCode == productCode);
        }

        public async Task<ProductResponse> Create(ProductRequest request)
        {
            try
            {
                // Find ProductType by ProductTypeCode - validate first like Java
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
                    Status = ProductStatus.instock, // Default status like Java
                    ProductTypeCode = request.ProductTypeCode,
                };

                // Debug: Log the product before saving
                Console.WriteLine($"Creating product: ProductCode={product.ProductCode}, ProductTypeCode={product.ProductTypeCode}");

                // Don't set navigation property to avoid EF confusion
                _context.Products.Add(product);

                // Debug: Log the SQL that will be executed
                Console.WriteLine("About to save changes...");
                await _context.SaveChangesAsync();

                return new ProductResponse
                {
                    ProductCode = product.ProductCode,
                    ProductName = product.ProductName,
                    Size = product.Size,
                    Color = product.Color,
                    Quantity = product.Quantity,
                    ProductTypeName = productType.ProductTypeName,
                    Status = product.Status
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating product: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                throw;
            }
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

            // Validate ProductType exists if ProductTypeCode is being changed
            if (product.ProductTypeCode != request.ProductTypeCode)
            {
                var productType = await _context.ProductTypes
                    .FirstOrDefaultAsync(pt => pt.ProductTypeCode == request.ProductTypeCode);

                if (productType == null)
                {
                    throw new KeyNotFoundException($"ProductType with code '{request.ProductTypeCode}' not found.");
                }
            }

            // Update properties (không update ProductCode nhưng cho phép update ProductType)
            product.ProductName = request.ProductName;
            product.Size = request.Size;
            product.Color = request.Color;
            product.Quantity = request.Quantity;
            product.ProductTypeCode = request.ProductTypeCode; // Allow ProductTypeCode update
            product.UpdatedAt = DateTime.Now; // Update timestamp

            await _context.SaveChangesAsync();

            // Reload ProductType to get updated information
            await _context.Entry(product).Reference(p => p.ProductType).LoadAsync();

            return new ProductResponse
            {
                ProductCode = product.ProductCode,
                ProductName = product.ProductName,
                Size = product.Size,
                Color = product.Color,
                Quantity = product.Quantity,
                ProductTypeName = product.ProductType?.ProductTypeName ?? "Unknown",
                Status = product.Status
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
