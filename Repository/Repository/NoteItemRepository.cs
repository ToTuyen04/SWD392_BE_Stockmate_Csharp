using Repository.Data;
using Repository.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class NoteItemRepository : INoteItemRepository
    {
        private readonly ApplicationDbContext _context;
        public NoteItemRepository(ApplicationDbContext context) => _context = context;
    }
}
