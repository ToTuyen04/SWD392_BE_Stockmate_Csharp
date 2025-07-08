using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _context;
        private CategoryRepository? _categoryRepo;
        private InvalidTokenRepository? _invalidTokenRepo;
        private NoteItemRepository? _noteItemRepo;
        private ProductRepository? _productRepo;
        private ProductTypeRepository? _productTypeRepo;
        private RoleRepository? _roleRepo;
        private StockCheckNoteRepository? _stockCheckNoteRepo;
        private StockCheckProductRepository? _stockCheckProductRepo;
        private StockTransactionRepository? _stockTransactionRepo;
        private UserRepository? _userRepo;
        private WarehouseRepository? _warehouseRepo;

        public UnitOfWork() => _context = new ApplicationDbContext();

        public ICategoryRepository CategoryRepository
        {
            get
            {
                return _categoryRepo ??= new CategoryRepository(_context);
            }
        }

        public IInvalidTokenRepository InvalidTokenRepository
        {
            get
            {
                return _invalidTokenRepo ??= new InvalidTokenRepository(_context);
            }
        }

        public INoteItemRepository NoteItemRepository
        {
            get
            {
                return _noteItemRepo ??= new NoteItemRepository(_context);
            }
        }

        public IProductRepository ProductRepository
        {
            get
            {
                return _productRepo ??= new ProductRepository(_context);
            }
        }

        public IProducTypetRepository producTypetRepository
        {
            get
            {
                return _productTypeRepo ??= new ProductTypeRepository(_context);
            }
        }

        public IRoleRepository RoleRepository
        {
            get
            {
                return _roleRepo ??= new RoleRepository(_context);
            }
        }

        public IStockCheckNoteRepository StockCheckNoteRepository
        {
            get
            {
                return _stockCheckNoteRepo ??= new StockCheckNoteRepository(_context);
            }
        }

        public IStockCheckProductRepository StockCheckProductRepository
        {
            get
            {
                return _stockCheckProductRepo ??= new StockCheckProductRepository(_context);
            }
        }

        public IStockTransactionRepository StockTransactionRepository
        {
            get
            {
                return _stockTransactionRepo ??= new StockTransactionRepository(_context);
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                return _userRepo ??= new UserRepository(_context);
            }
        }

        public IWarehouseRepository WarehouseRepository
        {
            get
            {
                return _warehouseRepo ??= new WarehouseRepository(_context);
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }

        public int SaveChangesWithTransaction()
        {
            int result = -1;
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    result = _context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }
    }
}
