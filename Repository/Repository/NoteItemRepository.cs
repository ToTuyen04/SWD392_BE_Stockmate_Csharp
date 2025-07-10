using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Models.Entities;
using Repository.Models.Enums;
using Repository.Repository.Interface;

namespace Repository.Repository
{
    public class NoteItemRepository : INoteItemRepository
    {
        private readonly ApplicationDbContext _context;
        public NoteItemRepository(ApplicationDbContext context) => _context = context;

        public async Task<int> GetTotalImportByProductAndWarehouse(string productCode, string warehouseCode)
        {
            var total = await _context.NoteItems
                .Where(ni => ni.Product.ProductCode == productCode
                          && ni.ExchangeNote.TransactionType == StockTransactionType.IMPORT
                          && ni.ExchangeNote.DestinationWarehouse.WarehouseCode == warehouseCode
                          && ni.Status == NoteItemStatus.COMPLETED)
                .SumAsync(ni => ni.Quantity);

            return total;
        }

        public async Task<int> GetTotalExportByProductAndWarehouse(string productCode, string warehouseCode)
        {
            var total = await _context.NoteItems
                .Where(ni => ni.Product.ProductCode == productCode
                          && ni.ExchangeNote.TransactionType == StockTransactionType.EXPORT
                          && ni.ExchangeNote.SourceWarehouse.WarehouseCode == warehouseCode
                          && ni.Status == NoteItemStatus.COMPLETED)
                .SumAsync(ni => ni.Quantity);

            return total;
        }

        public async Task<int> GetTotalImportAfterLastCheck(string productCode, string warehouseCode, DateTime lastStockCheckDate)
        {
            var total = await _context.NoteItems
                .Where(ni => ni.Product.ProductCode == productCode
                          && ni.ExchangeNote.TransactionType == StockTransactionType.IMPORT
                          && ni.ExchangeNote.DestinationWarehouse.WarehouseCode == warehouseCode
                          && ni.Status == NoteItemStatus.COMPLETED
                          && ni.ExchangeNote.Date > lastStockCheckDate)
                .SumAsync(ni => ni.Quantity);

            return total;
        }

        public async Task<int> GetTotalExportAfterLastCheck(string productCode, string warehouseCode, DateTime lastStockCheckDate)
        {
            var total = await _context.NoteItems
                .Where(ni => ni.Product.ProductCode == productCode
                          && ni.ExchangeNote.TransactionType == StockTransactionType.EXPORT
                          && ni.ExchangeNote.SourceWarehouse.WarehouseCode == warehouseCode
                          && ni.Status == NoteItemStatus.COMPLETED
                          && ni.ExchangeNote.Date > lastStockCheckDate)
                .SumAsync(ni => ni.Quantity);

            return total;
        }

        public async Task<int> GetTotalStockByProductAndWarehouse(string productCode, string warehouseCode)
        {
            var total = await _context.NoteItems
                .Where(ni => ni.Product.ProductCode == productCode
                          && (ni.ExchangeNote.DestinationWarehouse.WarehouseCode == warehouseCode
                           || ni.ExchangeNote.SourceWarehouse.WarehouseCode == warehouseCode)
                          && ni.ExchangeNote.Status == StockExchangeStatus.finished)
                .SumAsync(ni => ni.ExchangeNote.TransactionType == StockTransactionType.IMPORT ? ni.Quantity : -ni.Quantity);

            return total;
        }

        // Stock Transaction methods
        public async Task<NoteItemResponse> AddToTransaction(string exchangeNoteId, TransactionItemRequest request)
        {
            // Generate unique note item code
            var noteItemCode = "NI" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(100, 999);

            // Get exchange note
            var exchangeNote = await _context.ExchangeNotes
                .FirstOrDefaultAsync(en => en.ExchangeNoteId == exchangeNoteId);
            if (exchangeNote == null)
                throw new KeyNotFoundException($"Exchange note '{exchangeNoteId}' not found.");

            // Get product
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductCode == request.ProductCode);
            if (product == null)
                throw new KeyNotFoundException($"Product '{request.ProductCode}' not found.");

            // Create note item
            var noteItem = new NoteItem
            {
                NoteItemId = Guid.NewGuid().ToString(),
                NoteItemCode = noteItemCode,
                ProductCode = product.ProductCode,
                Product = product,
                ExchangeNoteId = exchangeNote.ExchangeNoteId,
                ExchangeNote = exchangeNote,
                Quantity = request.Quantity,
                Status = NoteItemStatus.PENDING
            };

            _context.NoteItems.Add(noteItem);
            await _context.SaveChangesAsync();

            return new NoteItemResponse
            {
                NoteItemCode = noteItem.NoteItemCode,
                ProductCode = product.ProductCode,
                ProductName = product.ProductName,
                Quantity = noteItem.Quantity
            };
        }

        public async Task<List<NoteItemResponse>> GetByTransactionId(string exchangeNoteId)
        {
            var noteItems = await _context.NoteItems
                .Include(ni => ni.Product)
                .Where(ni => ni.ExchangeNote.ExchangeNoteId == exchangeNoteId)
                .ToListAsync();

            return noteItems.Select(ni => new NoteItemResponse
            {
                NoteItemCode = ni.NoteItemCode,
                ProductCode = ni.Product.ProductCode,
                ProductName = ni.Product.ProductName,
                Quantity = ni.Quantity
            }).ToList();
        }

        public async Task<NoteItemResponse> UpdateQuantity(string noteItemCode, int quantity)
        {
            var noteItem = await _context.NoteItems
                .Include(ni => ni.Product)
                .FirstOrDefaultAsync(ni => ni.NoteItemCode == noteItemCode);

            if (noteItem == null)
                return null;

            noteItem.Quantity = quantity;
            await _context.SaveChangesAsync();

            return new NoteItemResponse
            {
                NoteItemCode = noteItem.NoteItemCode,
                ProductCode = noteItem.Product.ProductCode,
                ProductName = noteItem.Product.ProductName,
                Quantity = noteItem.Quantity
            };
        }

        public async Task<bool> Delete(string noteItemCode)
        {
            var noteItem = await _context.NoteItems
                .FirstOrDefaultAsync(ni => ni.NoteItemCode == noteItemCode);

            if (noteItem == null)
                return false;

            _context.NoteItems.Remove(noteItem);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
