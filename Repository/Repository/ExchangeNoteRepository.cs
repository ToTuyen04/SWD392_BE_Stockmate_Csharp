using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Models.Entities;
using Repository.Models.Enums;
using Repository.Repository.Interface;

namespace Repository.Repository
{
    public class ExchangeNoteRepository : IExchangeNoteRepository
    {
        private readonly ApplicationDbContext _context;
        public ExchangeNoteRepository(ApplicationDbContext context) => _context = context;

        public async Task<StockExchangeResponse> Create(StockExchangeRequest request)
        {
            // Generate unique ID
            var exchangeNoteId = "EXN" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(100, 999);

            // Get warehouses
            Warehouse sourceWarehouse = null;
            Warehouse destinationWarehouse = null;

            if (!string.IsNullOrEmpty(request.SourceWarehouseCode))
            {
                sourceWarehouse = await _context.Warehouses
                    .FirstOrDefaultAsync(w => w.WarehouseCode == request.SourceWarehouseCode);
            }

            if (!string.IsNullOrEmpty(request.DestinationWarehouseCode))
            {
                destinationWarehouse = await _context.Warehouses
                    .FirstOrDefaultAsync(w => w.WarehouseCode == request.DestinationWarehouseCode);
            }

            // Get creator user (use first available user as fallback)
            var creator = await _context.Users.FirstOrDefaultAsync();
            if (creator == null)
                throw new KeyNotFoundException("No users found in the system.");

            // Create exchange note
            var exchangeNote = new ExchangeNote
            {
                ExchangeNoteId = exchangeNoteId,
                Date = DateTime.Now,
                TransactionType = request.TransactionType,
                SourceWarehouseCode = sourceWarehouse?.WarehouseCode,
                SourceWarehouse = sourceWarehouse,
                DestinationWarehouseCode = destinationWarehouse?.WarehouseCode,
                DestinationWarehouse = destinationWarehouse,
                CreatedByUserCode = creator.UserCode,
                CreatedBy = creator,
                Status = StockExchangeStatus.pending
            };

            _context.ExchangeNotes.Add(exchangeNote);
            await _context.SaveChangesAsync();

            return new StockExchangeResponse
            {
                TransactionId = exchangeNote.ExchangeNoteId,
                TransactionType = exchangeNote.TransactionType,
                SourceWarehouseCode = sourceWarehouse?.WarehouseCode,
                DestinationWarehouseCode = destinationWarehouse?.WarehouseCode,
                CreatedBy = creator.UserCode,
                Status = exchangeNote.Status,
                Items = new List<NoteItemResponse>()
            };
        }

        public async Task<StockExchangeResponse> GetByCode(string exchangeNoteId)
        {
            var exchangeNote = await _context.ExchangeNotes
                .Include(en => en.SourceWarehouse)
                .Include(en => en.DestinationWarehouse)
                .Include(en => en.CreatedBy)
                .Include(en => en.ApprovedBy)
                .Include(en => en.NoteItems)
                    .ThenInclude(ni => ni.Product)
                .FirstOrDefaultAsync(en => en.ExchangeNoteId == exchangeNoteId);

            if (exchangeNote == null)
                return null;

            return new StockExchangeResponse
            {
                TransactionId = exchangeNote.ExchangeNoteId,
                TransactionType = exchangeNote.TransactionType,
                SourceWarehouseCode = exchangeNote.SourceWarehouse?.WarehouseCode,
                DestinationWarehouseCode = exchangeNote.DestinationWarehouse?.WarehouseCode,
                CreatedBy = exchangeNote.CreatedBy?.UserCode,
                ApprovedBy = exchangeNote.ApprovedBy?.UserCode,
                Status = exchangeNote.Status,
                Items = exchangeNote.NoteItems?.Select(ni => new NoteItemResponse
                {
                    NoteItemCode = ni.NoteItemCode,
                    ProductCode = ni.Product.ProductCode,
                    ProductName = ni.Product.ProductName,
                    Quantity = ni.Quantity
                }).ToList() ?? new List<NoteItemResponse>()
            };
        }

        public async Task<List<StockExchangeResponse>> GetAll()
        {
            var exchangeNotes = await _context.ExchangeNotes
                .Include(en => en.SourceWarehouse)
                .Include(en => en.DestinationWarehouse)
                .Include(en => en.CreatedBy)
                .Include(en => en.ApprovedBy)
                .Include(en => en.NoteItems)
                    .ThenInclude(ni => ni.Product)
                .OrderByDescending(en => en.Date)
                .ToListAsync();

            return exchangeNotes.Select(en => new StockExchangeResponse
            {
                TransactionId = en.ExchangeNoteId,
                TransactionType = en.TransactionType,
                SourceWarehouseCode = en.SourceWarehouse?.WarehouseCode,
                DestinationWarehouseCode = en.DestinationWarehouse?.WarehouseCode,
                CreatedBy = en.CreatedBy?.UserCode,
                ApprovedBy = en.ApprovedBy?.UserCode,
                Status = en.Status,
                Items = en.NoteItems?.Select(ni => new NoteItemResponse
                {
                    NoteItemCode = ni.NoteItemCode,
                    ProductCode = ni.Product.ProductCode,
                    ProductName = ni.Product.ProductName,
                    Quantity = ni.Quantity
                }).ToList() ?? new List<NoteItemResponse>()
            }).ToList();
        }

        public async Task<List<StockExchangeResponse>> GetByWarehouse(string warehouseCode)
        {
            var exchangeNotes = await _context.ExchangeNotes
                .Include(en => en.SourceWarehouse)
                .Include(en => en.DestinationWarehouse)
                .Include(en => en.CreatedBy)
                .Include(en => en.ApprovedBy)
                .Include(en => en.NoteItems)
                    .ThenInclude(ni => ni.Product)
                .Where(en => en.SourceWarehouse.WarehouseCode == warehouseCode ||
                           en.DestinationWarehouse.WarehouseCode == warehouseCode)
                .OrderByDescending(en => en.Date)
                .ToListAsync();

            return exchangeNotes.Select(en => new StockExchangeResponse
            {
                TransactionId = en.ExchangeNoteId,
                TransactionType = en.TransactionType,
                SourceWarehouseCode = en.SourceWarehouse?.WarehouseCode,
                DestinationWarehouseCode = en.DestinationWarehouse?.WarehouseCode,
                CreatedBy = en.CreatedBy?.UserCode,
                ApprovedBy = en.ApprovedBy?.UserCode,
                Status = en.Status,
                Items = en.NoteItems?.Select(ni => new NoteItemResponse
                {
                    NoteItemCode = ni.NoteItemCode,
                    ProductCode = ni.Product.ProductCode,
                    ProductName = ni.Product.ProductName,
                    Quantity = ni.Quantity
                }).ToList() ?? new List<NoteItemResponse>()
            }).ToList();
        }

        public async Task<List<StockExchangeResponse>> GetByStatus(StockExchangeStatus status)
        {
            var exchangeNotes = await _context.ExchangeNotes
                .Include(en => en.SourceWarehouse)
                .Include(en => en.DestinationWarehouse)
                .Include(en => en.CreatedBy)
                .Include(en => en.ApprovedBy)
                .Include(en => en.NoteItems)
                    .ThenInclude(ni => ni.Product)
                .Where(en => en.Status == status)
                .OrderByDescending(en => en.Date)
                .ToListAsync();

            return exchangeNotes.Select(en => new StockExchangeResponse
            {
                TransactionId = en.ExchangeNoteId,
                TransactionType = en.TransactionType,
                SourceWarehouseCode = en.SourceWarehouse?.WarehouseCode,
                DestinationWarehouseCode = en.DestinationWarehouse?.WarehouseCode,
                CreatedBy = en.CreatedBy?.UserCode,
                ApprovedBy = en.ApprovedBy?.UserCode,
                Status = en.Status,
                Items = en.NoteItems?.Select(ni => new NoteItemResponse
                {
                    NoteItemCode = ni.NoteItemCode,
                    ProductCode = ni.Product.ProductCode,
                    ProductName = ni.Product.ProductName,
                    Quantity = ni.Quantity
                }).ToList() ?? new List<NoteItemResponse>()
            }).ToList();
        }

        public async Task<StockExchangeResponse> UpdateStatus(string exchangeNoteId, StockExchangeStatus status)
        {
            var exchangeNote = await _context.ExchangeNotes
                .Include(en => en.SourceWarehouse)
                .Include(en => en.DestinationWarehouse)
                .Include(en => en.CreatedBy)
                .Include(en => en.ApprovedBy)
                .Include(en => en.NoteItems)
                    .ThenInclude(ni => ni.Product)
                .FirstOrDefaultAsync(en => en.ExchangeNoteId == exchangeNoteId);

            if (exchangeNote == null)
                return null;

            exchangeNote.Status = status;
            await _context.SaveChangesAsync();

            return new StockExchangeResponse
            {
                TransactionId = exchangeNote.ExchangeNoteId,
                TransactionType = exchangeNote.TransactionType,
                SourceWarehouseCode = exchangeNote.SourceWarehouse?.WarehouseCode,
                DestinationWarehouseCode = exchangeNote.DestinationWarehouse?.WarehouseCode,
                CreatedBy = exchangeNote.CreatedBy?.UserCode,
                ApprovedBy = exchangeNote.ApprovedBy?.UserCode,
                Status = exchangeNote.Status,
                Items = exchangeNote.NoteItems?.Select(ni => new NoteItemResponse
                {
                    NoteItemCode = ni.NoteItemCode,
                    ProductCode = ni.Product.ProductCode,
                    ProductName = ni.Product.ProductName,
                    Quantity = ni.Quantity
                }).ToList() ?? new List<NoteItemResponse>()
            };
        }

        public async Task<StockExchangeResponse> Approve(string exchangeNoteId, string approvedBy)
        {
            var exchangeNote = await _context.ExchangeNotes
                .Include(en => en.SourceWarehouse)
                .Include(en => en.DestinationWarehouse)
                .Include(en => en.CreatedBy)
                .Include(en => en.NoteItems)
                    .ThenInclude(ni => ni.Product)
                .FirstOrDefaultAsync(en => en.ExchangeNoteId == exchangeNoteId);

            if (exchangeNote == null)
                return null;

            // Get approver user
            var approver = await _context.Users.FirstOrDefaultAsync(u => u.UserCode == approvedBy);
            if (approver == null)
                approver = await _context.Users.FirstOrDefaultAsync(); // Fallback to first user

            exchangeNote.Status = StockExchangeStatus.accepted;
            exchangeNote.ApprovedByUserCode = approver?.UserCode;
            exchangeNote.ApprovedBy = approver;
            await _context.SaveChangesAsync();

            return new StockExchangeResponse
            {
                TransactionId = exchangeNote.ExchangeNoteId,
                TransactionType = exchangeNote.TransactionType,
                SourceWarehouseCode = exchangeNote.SourceWarehouse?.WarehouseCode,
                DestinationWarehouseCode = exchangeNote.DestinationWarehouse?.WarehouseCode,
                CreatedBy = exchangeNote.CreatedBy?.UserCode,
                ApprovedBy = exchangeNote.ApprovedBy?.UserCode,
                Status = exchangeNote.Status,
                Items = exchangeNote.NoteItems?.Select(ni => new NoteItemResponse
                {
                    NoteItemCode = ni.NoteItemCode,
                    ProductCode = ni.Product.ProductCode,
                    ProductName = ni.Product.ProductName,
                    Quantity = ni.Quantity
                }).ToList() ?? new List<NoteItemResponse>()
            };
        }

        public async Task<bool> Delete(string exchangeNoteId)
        {
            var exchangeNote = await _context.ExchangeNotes
                .FirstOrDefaultAsync(en => en.ExchangeNoteId == exchangeNoteId);

            if (exchangeNote == null)
                return false;

            _context.ExchangeNotes.Remove(exchangeNote);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
