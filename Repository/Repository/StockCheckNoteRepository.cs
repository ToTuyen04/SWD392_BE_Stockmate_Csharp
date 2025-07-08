using Repository.Data;
using Repository.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class StockCheckNoteRepository : IStockCheckNoteRepository
    {
        private readonly ApplicationDbContext _context;
        public StockCheckNoteRepository(ApplicationDbContext context) => _context = context;
    }
}
