using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Models.Entities;
using Repository.Models.Enums;
using Repository.Repository.Interface;

namespace Repository.Repository
{
    public class StockCheckNoteRepository : IStockCheckNoteRepository
    {
        private readonly ApplicationDbContext _context;
        public StockCheckNoteRepository(ApplicationDbContext context) => _context = context;

        public async Task<StockCheckNoteResponse> Create(StockCheckNoteRequest request, string checkerUserCode)
        {
            // Generate unique ID
            var stockCheckNoteId = "SCN" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999);

            // Get warehouse and checker
            var warehouse = await _context.Warehouses
                .FirstOrDefaultAsync(w => w.WarehouseCode == request.WarehouseCode);

            if (warehouse == null)
                throw new KeyNotFoundException($"Warehouse with code '{request.WarehouseCode}' not found.");

            // Try to get the checker user, if not found, use the first available user
            var checker = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserCode == checkerUserCode);

            if (checker == null)
            {
                // Use the first available user as fallback
                checker = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync();

                if (checker == null)
                    throw new KeyNotFoundException("No users found in the system.");
            }

            // Create stock check note
            var stockCheckNote = new StockCheckNote
            {
                StockCheckNoteId = stockCheckNoteId,
                DateTime = DateTime.Now,
                WarehouseCode = warehouse.WarehouseCode,
                Warehouse = warehouse,
                CheckerUserCode = checker.UserCode,
                Checker = checker,
                Description = request.Description ?? "",
                StockCheckStatus = StockCheckStatus.pending,
                StockCheckProducts = new List<StockCheckProduct>()
            };

            _context.StockCheckNotes.Add(stockCheckNote);
            await _context.SaveChangesAsync();

            return new StockCheckNoteResponse
            {
                StockCheckNoteId = stockCheckNote.StockCheckNoteId,
                DateTime = stockCheckNote.DateTime,
                WarehouseCode = warehouse.WarehouseCode,
                WarehouseName = warehouse.WarehouseName,
                CheckerName = checker.FullName,
                StockCheckStatus = stockCheckNote.StockCheckStatus.ToString(),
                StockCheckProducts = new List<StockCheckProductResponse>()
            };
        }

        public async Task<List<StockCheckNoteResponse>> GetAll()
        {
            var stockCheckNotes = await _context.StockCheckNotes
                .Include(scn => scn.Warehouse)
                .Include(scn => scn.Checker)
                .OrderByDescending(scn => scn.DateTime)
                .ToListAsync();

            var result = new List<StockCheckNoteResponse>();

            foreach (var scn in stockCheckNotes)
            {
                // Load stock check products separately to avoid complex joins
                var stockCheckProducts = await _context.StockCheckProducts
                    .Where(scp => scp.StockCheckNoteId == scn.StockCheckNoteId)
                    .ToListAsync();

                var stockCheckProductResponses = new List<StockCheckProductResponse>();

                foreach (var scp in stockCheckProducts)
                {
                    var product = await _context.Products
                        .FirstOrDefaultAsync(p => p.ProductCode == scp.ProductCode);

                    if (product != null)
                    {
                        stockCheckProductResponses.Add(new StockCheckProductResponse
                        {
                            ProductCode = product.ProductCode,
                            ProductName = product.ProductName,
                            LastQuantity = scp.LastQuantity,
                            ActualQuantity = scp.ActualQuantity,
                            ExpectedQuantity = scp.ExpectedQuantity,
                            TotalImportQuantity = scp.TotalImportQuantity,
                            TotalExportQuantity = scp.TotalExportQuantity,
                            Difference = scp.Difference
                        });
                    }
                }

                result.Add(new StockCheckNoteResponse
                {
                    StockCheckNoteId = scn.StockCheckNoteId,
                    DateTime = scn.DateTime,
                    WarehouseCode = scn.Warehouse.WarehouseCode,
                    WarehouseName = scn.Warehouse.WarehouseName,
                    CheckerName = scn.Checker.FullName,
                    StockCheckStatus = scn.StockCheckStatus.ToString(),
                    StockCheckProducts = stockCheckProductResponses
                });
            }

            return result;
        }

        public async Task<List<StockCheckNoteResponse>> GetByWarehouse(string warehouseCode)
        {
            var stockCheckNotes = await _context.StockCheckNotes
                .Include(scn => scn.Warehouse)
                .Include(scn => scn.Checker)
                .Include(scn => scn.StockCheckProducts)
                    .ThenInclude(scp => scp.Product)
                .Where(scn => scn.Warehouse.WarehouseCode == warehouseCode)
                .OrderByDescending(scn => scn.DateTime)
                .ToListAsync();

            return stockCheckNotes.Select(scn => new StockCheckNoteResponse
            {
                StockCheckNoteId = scn.StockCheckNoteId,
                DateTime = scn.DateTime,
                WarehouseCode = scn.Warehouse.WarehouseCode,
                WarehouseName = scn.Warehouse.WarehouseName,
                CheckerName = scn.Checker.FullName,
                StockCheckStatus = scn.StockCheckStatus.ToString(),
                StockCheckProducts = scn.StockCheckProducts?.Select(scp => new StockCheckProductResponse
                {
                    ProductCode = scp.Product.ProductCode,
                    ProductName = scp.Product.ProductName,
                    LastQuantity = scp.LastQuantity,
                    ActualQuantity = scp.ActualQuantity,
                    ExpectedQuantity = scp.ExpectedQuantity,
                    TotalImportQuantity = scp.TotalImportQuantity,
                    TotalExportQuantity = scp.TotalExportQuantity
                }).ToList() ?? new List<StockCheckProductResponse>()
            }).ToList();
        }

        public async Task<StockCheckNoteResponse> GetByCode(string stockCheckNoteId)
        {
            var stockCheckNote = await _context.StockCheckNotes
                .Include(scn => scn.Warehouse)
                .Include(scn => scn.Checker)
                .Include(scn => scn.StockCheckProducts)
                    .ThenInclude(scp => scp.Product)
                .FirstOrDefaultAsync(scn => scn.StockCheckNoteId == stockCheckNoteId);

            if (stockCheckNote == null)
                throw new KeyNotFoundException($"Stock check note with ID '{stockCheckNoteId}' not found.");

            return new StockCheckNoteResponse
            {
                StockCheckNoteId = stockCheckNote.StockCheckNoteId,
                DateTime = stockCheckNote.DateTime,
                WarehouseCode = stockCheckNote.Warehouse.WarehouseCode,
                WarehouseName = stockCheckNote.Warehouse.WarehouseName,
                CheckerName = stockCheckNote.Checker.FullName,
                StockCheckStatus = stockCheckNote.StockCheckStatus.ToString(),
                StockCheckProducts = stockCheckNote.StockCheckProducts?.Select(scp => new StockCheckProductResponse
                {
                    ProductCode = scp.Product.ProductCode,
                    ProductName = scp.Product.ProductName,
                    LastQuantity = scp.LastQuantity,
                    ActualQuantity = scp.ActualQuantity,
                    ExpectedQuantity = scp.ExpectedQuantity,
                    TotalImportQuantity = scp.TotalImportQuantity,
                    TotalExportQuantity = scp.TotalExportQuantity
                }).ToList() ?? new List<StockCheckProductResponse>()
            };
        }

        public async Task<StockCheckNoteResponse> Update(string stockCheckNoteId, StockCheckStatus status)
        {
            var stockCheckNote = await _context.StockCheckNotes
                .Include(scn => scn.Warehouse)
                .Include(scn => scn.Checker)
                .Include(scn => scn.StockCheckProducts)
                    .ThenInclude(scp => scp.Product)
                .FirstOrDefaultAsync(scn => scn.StockCheckNoteId == stockCheckNoteId);

            if (stockCheckNote == null)
                throw new KeyNotFoundException($"Stock check note with ID '{stockCheckNoteId}' not found.");

            stockCheckNote.StockCheckStatus = status;
            await _context.SaveChangesAsync();

            return new StockCheckNoteResponse
            {
                StockCheckNoteId = stockCheckNote.StockCheckNoteId,
                DateTime = stockCheckNote.DateTime,
                WarehouseCode = stockCheckNote.Warehouse.WarehouseCode,
                WarehouseName = stockCheckNote.Warehouse.WarehouseName,
                CheckerName = stockCheckNote.Checker.FullName,
                StockCheckStatus = stockCheckNote.StockCheckStatus.ToString(),
                StockCheckProducts = stockCheckNote.StockCheckProducts?.Select(scp => new StockCheckProductResponse
                {
                    ProductCode = scp.Product.ProductCode,
                    ProductName = scp.Product.ProductName,
                    LastQuantity = scp.LastQuantity,
                    ActualQuantity = scp.ActualQuantity,
                    ExpectedQuantity = scp.ExpectedQuantity,
                    TotalImportQuantity = scp.TotalImportQuantity,
                    TotalExportQuantity = scp.TotalExportQuantity
                }).ToList() ?? new List<StockCheckProductResponse>()
            };
        }

        public async Task<List<StockCheckNoteResponse>> GetByStatus(StockCheckStatus status)
        {
            var stockCheckNotes = await _context.StockCheckNotes
                .Include(scn => scn.Warehouse)
                .Include(scn => scn.Checker)
                .Include(scn => scn.StockCheckProducts)
                    .ThenInclude(scp => scp.Product)
                .Where(scn => scn.StockCheckStatus == status)
                .OrderByDescending(scn => scn.DateTime)
                .ToListAsync();

            return stockCheckNotes.Select(scn => new StockCheckNoteResponse
            {
                StockCheckNoteId = scn.StockCheckNoteId,
                DateTime = scn.DateTime,
                WarehouseCode = scn.Warehouse.WarehouseCode,
                WarehouseName = scn.Warehouse.WarehouseName,
                CheckerName = scn.Checker.FullName,
                StockCheckStatus = scn.StockCheckStatus.ToString(),
                StockCheckProducts = scn.StockCheckProducts?.Select(scp => new StockCheckProductResponse
                {
                    ProductCode = scp.Product.ProductCode,
                    ProductName = scp.Product.ProductName,
                    LastQuantity = scp.LastQuantity,
                    ActualQuantity = scp.ActualQuantity,
                    ExpectedQuantity = scp.ExpectedQuantity,
                    TotalImportQuantity = scp.TotalImportQuantity,
                    TotalExportQuantity = scp.TotalExportQuantity
                }).ToList() ?? new List<StockCheckProductResponse>()
            }).ToList();
        }

        public async Task<StockCheckNote> GetEntityByCode(string stockCheckNoteId)
        {
            var stockCheckNote = await _context.StockCheckNotes
                .Include(scn => scn.Warehouse)
                .Include(scn => scn.Checker)
                .Include(scn => scn.StockCheckProducts)
                    .ThenInclude(scp => scp.Product)
                .FirstOrDefaultAsync(scn => scn.StockCheckNoteId == stockCheckNoteId);

            if (stockCheckNote == null)
                throw new KeyNotFoundException($"Stock check note with ID '{stockCheckNoteId}' not found.");

            return stockCheckNote;
        }
    }
}
