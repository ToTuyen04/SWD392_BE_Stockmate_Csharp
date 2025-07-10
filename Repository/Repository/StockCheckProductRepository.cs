using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Models.Entities;
using Repository.Models.Enums;
using Repository.Repository.Interface;

namespace Repository.Repository
{
    public class StockCheckProductRepository : IStockCheckProductRepository
    {
        private readonly ApplicationDbContext _context;
        public StockCheckProductRepository(ApplicationDbContext context) => _context = context;

        public async Task<List<StockCheckProductResponse>> GetByStockCheckNote(string stockCheckNoteId)
        {
            var stockCheckProducts = await _context.StockCheckProducts
                .Include(scp => scp.Product)
                .Include(scp => scp.StockCheckNote)
                .Where(scp => scp.StockCheckNote.StockCheckNoteId == stockCheckNoteId)
                .ToListAsync();

            return stockCheckProducts.Select(scp => new StockCheckProductResponse
            {
                ProductCode = scp.Product.ProductCode,
                ProductName = scp.Product.ProductName,
                LastQuantity = scp.LastQuantity,
                ActualQuantity = scp.ActualQuantity,
                ExpectedQuantity = scp.ExpectedQuantity,
                TotalImportQuantity = scp.TotalImportQuantity,
                TotalExportQuantity = scp.TotalExportQuantity,
                Difference = scp.Difference
            }).ToList();
        }

        public async Task<StockCheckProductResponse> UpdateActualQuantity(string stockCheckNoteId, string productCode, int actualQuantity)
        {
            var stockCheckProduct = await _context.StockCheckProducts
                .Include(scp => scp.Product)
                .Include(scp => scp.StockCheckNote)
                .FirstOrDefaultAsync(scp => scp.StockCheckNote.StockCheckNoteId == stockCheckNoteId
                                         && scp.Product.ProductCode == productCode);

            if (stockCheckProduct == null)
                throw new KeyNotFoundException($"Stock check product not found for note '{stockCheckNoteId}' and product '{productCode}'.");

            stockCheckProduct.ActualQuantity = actualQuantity;
            stockCheckProduct.CalculateTheoreticalQuantity(); // This will also calculate difference
            await _context.SaveChangesAsync();

            return new StockCheckProductResponse
            {
                ProductCode = stockCheckProduct.Product.ProductCode,
                ProductName = stockCheckProduct.Product.ProductName,
                LastQuantity = stockCheckProduct.LastQuantity,
                ActualQuantity = stockCheckProduct.ActualQuantity,
                ExpectedQuantity = stockCheckProduct.ExpectedQuantity,
                TotalImportQuantity = stockCheckProduct.TotalImportQuantity,
                TotalExportQuantity = stockCheckProduct.TotalExportQuantity,
                Difference = stockCheckProduct.Difference
            };
        }

        public async Task<StockCheckProductResponse> AddToStockCheck(string stockCheckNoteId, StockCheckProductRequest request)
        {
            // Get stock check note
            var stockCheckNote = await _context.StockCheckNotes
                .Include(scn => scn.Warehouse)
                .FirstOrDefaultAsync(scn => scn.StockCheckNoteId == stockCheckNoteId);

            if (stockCheckNote == null)
                throw new KeyNotFoundException($"Stock check note with ID '{stockCheckNoteId}' not found.");

            // Get product
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductCode == request.ProductCode);

            if (product == null)
                throw new KeyNotFoundException($"Product with code '{request.ProductCode}' not found.");

            // Check if product already exists in this stock check
            var existingProduct = await _context.StockCheckProducts
                .FirstOrDefaultAsync(scp => scp.StockCheckNote.StockCheckNoteId == stockCheckNoteId
                                         && scp.Product.ProductCode == request.ProductCode);

            if (existingProduct != null)
                throw new InvalidOperationException($"Product '{request.ProductCode}' already exists in this stock check note.");

            // Get last stock check quantity and calculate import/export
            var lastStockCheck = await GetLatestStockCheck(request.ProductCode, stockCheckNote.Warehouse.WarehouseCode);
            var lastQuantity = lastStockCheck?.ActualQuantity ?? 0;

            // Calculate import/export quantities (simplified - you may need to implement proper calculation)
            var totalImport = 0; // TODO: Calculate from NoteItem repository
            var totalExport = 0; // TODO: Calculate from NoteItem repository

            // Generate unique ID
            var stockCheckProductId = "SCP" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999);

            // Create stock check product
            var stockCheckProduct = new StockCheckProduct
            {
                StockCheckProductId = stockCheckProductId,
                StockCheckNoteId = stockCheckNote.StockCheckNoteId,
                StockCheckNote = stockCheckNote,
                ProductCode = product.ProductCode,
                Product = product,
                LastQuantity = lastQuantity,
                ActualQuantity = request.ActualQuantity,
                TotalImportQuantity = totalImport,
                TotalExportQuantity = totalExport,
                ExpectedQuantity = lastQuantity + totalImport - totalExport,
                StockCheckProductStatus = StockCheckProductStatus.temporary
            };

            // Calculate difference
            stockCheckProduct.CalculateTheoreticalQuantity();

            _context.StockCheckProducts.Add(stockCheckProduct);
            await _context.SaveChangesAsync();

            return new StockCheckProductResponse
            {
                ProductCode = stockCheckProduct.Product.ProductCode,
                ProductName = stockCheckProduct.Product.ProductName,
                LastQuantity = stockCheckProduct.LastQuantity,
                ActualQuantity = stockCheckProduct.ActualQuantity,
                ExpectedQuantity = stockCheckProduct.ExpectedQuantity,
                TotalImportQuantity = stockCheckProduct.TotalImportQuantity,
                TotalExportQuantity = stockCheckProduct.TotalExportQuantity,
                Difference = stockCheckProduct.Difference
            };
        }

        public async Task RemoveFromStockCheck(string stockCheckNoteId, string productCode)
        {
            var stockCheckProduct = await _context.StockCheckProducts
                .Include(scp => scp.Product)
                .Include(scp => scp.StockCheckNote)
                .FirstOrDefaultAsync(scp => scp.StockCheckNote.StockCheckNoteId == stockCheckNoteId
                                         && scp.Product.ProductCode == productCode);

            if (stockCheckProduct == null)
                throw new KeyNotFoundException($"Stock check product not found for note '{stockCheckNoteId}' and product '{productCode}'.");

            _context.StockCheckProducts.Remove(stockCheckProduct);
            await _context.SaveChangesAsync();
        }

        public async Task<StockCheckProduct> GetLatestStockCheck(string productCode, string warehouseCode)
        {
            return await _context.StockCheckProducts
                .Include(scp => scp.Product)
                .Include(scp => scp.StockCheckNote)
                    .ThenInclude(scn => scn.Warehouse)
                .Where(scp => scp.Product.ProductCode == productCode
                           && scp.StockCheckNote.Warehouse.WarehouseCode == warehouseCode
                           && scp.StockCheckNote.StockCheckStatus == StockCheckStatus.finished)
                .OrderByDescending(scp => scp.StockCheckNote.DateTime)
                .FirstOrDefaultAsync();
        }

        public async Task<List<StockCheckProduct>> GetByStockCheckNoteAndStatus(string stockCheckNoteId, StockCheckProductStatus status)
        {
            return await _context.StockCheckProducts
                .Include(scp => scp.Product)
                .Include(scp => scp.StockCheckNote)
                .Where(scp => scp.StockCheckNote.StockCheckNoteId == stockCheckNoteId
                           && scp.StockCheckProductStatus == status)
                .ToListAsync();
        }

        public async Task UpdateProductsStatus(List<StockCheckProduct> products, StockCheckProductStatus status)
        {
            foreach (var product in products)
            {
                product.StockCheckProductStatus = status;
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProducts(List<StockCheckProduct> products)
        {
            _context.StockCheckProducts.RemoveRange(products);
            await _context.SaveChangesAsync();
        }
    }
}
