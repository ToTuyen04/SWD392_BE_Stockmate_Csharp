using Repository.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.Interface
{
    public interface IUnitOfWork :IDisposable
    {
        ICategoryRepository CategoryRepository { get; }
        IInvalidTokenRepository InvalidTokenRepository { get; }
        INoteItemRepository NoteItemRepository { get; }
        IProductRepository ProductRepository { get; }
        IProductTypeRepository ProductTypeRepository { get; }
        IRoleRepository RoleRepository { get; }
        IStockCheckNoteRepository StockCheckNoteRepository { get; }
        IStockCheckProductRepository StockCheckProductRepository { get; }
        IStockTransactionRepository StockTransactionRepository { get; }
        IUserRepository UserRepository { get; }
        IWarehouseRepository WarehouseRepository { get; }
        IExchangeNoteRepository ExchangeNoteRepository { get; }

        int SaveChangesWithTransaction();

    }
}
