using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Models.Exceptions;
using Repository.Models.Enums;
using Repository.Repository.Interface;
using Repository.Repository;
using Service.Service.Interface;

namespace Service.Service
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductTypeService() => _unitOfWork = new UnitOfWork();

        public async Task<ProductTypeResponse> CreateProductType(ProductTypeRequest request)
        {
            // Check if ProductType code already exists - exactly like Java
            var existingProductType = await _unitOfWork.ProductTypeRepository.GetEntityByCode(request.ProductTypeCode);
            if (existingProductType != null)
            {
                throw new AppException(ErrorCode.PRODUCT_TYPE_CODE_EXIST);
            }

            // Create ProductType using repository - let repository handle Category validation
            try
            {
                var createdProductType = await _unitOfWork.ProductTypeRepository.Create(request);
                return createdProductType;
            }
            catch (KeyNotFoundException ex) when (ex.Message.Contains("Category"))
            {
                throw new AppException(ErrorCode.CATEGORY_NOT_FOUND);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("already exists"))
            {
                // This shouldn't happen since we check above, but just in case
                throw new AppException(ErrorCode.PRODUCT_TYPE_CODE_EXIST);
            }
        }

        public async Task<ProductTypeResponse> GetProductTypeByCode(string productTypeCode)
        {
            try
            {
                var productType = await _unitOfWork.ProductTypeRepository.GetByCode(productTypeCode);
                return productType;
            }
            catch (KeyNotFoundException)
            {
                throw new AppException(ErrorCode.PRODUCT_TYPE_NOT_FOUND);
            }
        }

        public async Task<List<ProductTypeResponse>> GetAllProductTypes()
        {
            try
            {
                var productTypes = await _unitOfWork.ProductTypeRepository.GetAll();
                return productTypes;
            }
            catch (Exception)
            {
                throw new AppException(ErrorCode.UNKNOWN_ERROR);
            }
        }

        public async Task<ProductTypeResponse> UpdateProductType(string productTypeCode, ProductTypeRequest request)
        {
            try
            {
                var updatedProductType = await _unitOfWork.ProductTypeRepository.Update(productTypeCode, request);
                return updatedProductType;
            }
            catch (KeyNotFoundException ex) when (ex.Message.Contains("ProductType"))
            {
                throw new AppException(ErrorCode.PRODUCT_TYPE_NOT_FOUND);
            }
            catch (KeyNotFoundException ex) when (ex.Message.Contains("Category"))
            {
                throw new AppException(ErrorCode.CATEGORY_NOT_FOUND);
            }
        }

        public async Task DeleteProductType(string productTypeCode)
        {
            try
            {
                await _unitOfWork.ProductTypeRepository.Delete(productTypeCode);
            }
            catch (KeyNotFoundException)
            {
                throw new AppException(ErrorCode.PRODUCT_TYPE_NOT_FOUND);
            }
        }
    }
}
