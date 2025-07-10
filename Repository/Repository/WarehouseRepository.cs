using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Models.Entities;
using Repository.Repository.Interface;

namespace Repository.Repository
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly ApplicationDbContext _context;
        public WarehouseRepository(ApplicationDbContext context) => _context = context;

        public async Task<Warehouse> GetByCode(string warehouseCode)
        {
            return await _context.Warehouses
                .FirstOrDefaultAsync(w => w.WarehouseCode == warehouseCode);
        }
    }
}
