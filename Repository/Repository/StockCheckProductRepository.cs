using Repository.Data;
using Repository.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class StockCheckProductRepository : IStockCheckProductRepository
    {
        private readonly ApplicationDbContext _context;
        public StockCheckProductRepository(ApplicationDbContext context) => _context = context;
    }
}
