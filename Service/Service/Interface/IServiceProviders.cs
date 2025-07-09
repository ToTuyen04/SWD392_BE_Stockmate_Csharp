using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service.Interface
{
    public interface IServiceProviders
    {
        IAuthenticationService AuthenticationService { get; }
        ICategoryService CategoryService { get; }
        IInvalidTokenService InvalidTokenService { get; }
        INoteItemService NoteItemService { get; }
        IProductService ProductService { get; }
        IProductTypeService ProductTypeService { get; }
        IRoleService RoleService { get; }
        IStockCheckNoteService StockCheckNoteService { get; }
        IStockCheckProductService StockCheckProductService { get; }
        IStockTransactionService StockTransactionService { get; }
        IUserService UserService { get; }
        IWarehouseService WarehouseService { get; }
    }
}
