using Repository.Repository.Interface;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class ServiceProviders : IServiceProviders
    {
        private readonly IUnitOfWork _unitOfWork;
        private IAuthenticationService _authenticationService;
        private ICategoryService _categoryService;
        private IInvalidTokenService _invalidTokenService;
        private INoteItemService _noteItemService;
        private IProductService _productService;
        private IProductTypeService _productTypeService;
        private IRoleService _roleService;
        private IStockCheckService _stockCheckService;
        private IStockTransactionService _stockTransactionService;
        private IUserService _userService;
        private IWarehouseService _warehouseService;

        public ServiceProviders() { }

        public ICategoryService CategoryService
        {
            get
            {
                return _categoryService ??= new CategoryService();
            }
        }

        public IInvalidTokenService InvalidTokenService
        {
            get
            {
                return _invalidTokenService ??= new InvalidTokenService();
            }
        }

        public INoteItemService NoteItemService
        {
            get
            {
                return _noteItemService ??= new NoteItemService();
            }
        }

        public IProductService ProductService
        {
            get
            {
                return _productService ??= new ProductService();
            }
        }

        public IProductTypeService ProductTypeService
        {
            get
            {
                return _productTypeService ??= new ProductTypeService();
            }
        }

        public IRoleService RoleService
        {
            get
            {
                return _roleService ??= new RoleService();
            }
        }

        public IStockCheckService StockCheckService
        {
            get
            {
                return _stockCheckService ??= new StockCheckService();
            }
        }

        public IStockTransactionService StockTransactionService
        {
            get
            {
                return _stockTransactionService ??= new StockTransactionService();
            }
        }

        public IUserService UserService
        {
            get
            {
                return _userService ??= new UserService();
            }
        }

        public IWarehouseService WarehouseService
        {
            get
            {
                return _warehouseService ??= new WarehouseService();
            }
        }

        public IAuthenticationService AuthenticationService
        {
            get
            {
                return _authenticationService ??= new AuthenticationService();
            }
        }
    }
}
