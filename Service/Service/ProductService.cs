using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Models.Exceptions;
using Repository.Models.Enums;
using Repository.Repository.Interface;
using Repository.Repository;
using Service.Service.Interface;

namespace Service.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService() => _unitOfWork = new UnitOfWork();

        public async Task<ProductResponse> CreateProduct(ProductRequest request)
        {
            try
            {
                // Check if product code already exists - exactly like Java
                var existingProduct = await _unitOfWork.ProductRepository.GetEntityByCode(request.ProductCode);
                if (existingProduct != null)
                {
                    throw new AppException(ErrorCode.PRODUCT_CODE_EXIST);
                }

                // Create product using repository - let repository handle ProductType validation
                var createdProduct = await _unitOfWork.ProductRepository.Create(request);
                return createdProduct;
            }
            catch (KeyNotFoundException ex) when (ex.Message.Contains("ProductType"))
            {
                throw new AppException(ErrorCode.PRODUCT_TYPE_NOT_FOUND);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ProductService.CreateProduct: {ex.Message}");
                Console.WriteLine($"Request: ProductCode={request.ProductCode}, ProductTypeCode={request.ProductTypeCode}");
                throw new Exception("An error occurred while creating the product.", ex);
            }

        }

        public async Task<ProductResponse> GetProductByCode(string productCode)
        {
            var product = await _unitOfWork.ProductRepository.GetByCode(productCode);
            if (product == null)
            {
                throw new AppException(ErrorCode.PRODUCT_NOT_FOUND);
            }
            return product;
        }

        public async Task<List<ProductResponse>> GetAllProducts()
        {
            return await _unitOfWork.ProductRepository.GetAll();
        }

        public async Task<ProductResponse> UpdateProduct(string productCode, ProductRequest request)
        {
            try
            {
                var updatedProduct = await _unitOfWork.ProductRepository.Update(productCode, request);
                return updatedProduct;
            }
            catch (KeyNotFoundException)
            {
                throw new AppException(ErrorCode.PRODUCT_NOT_FOUND);
            }
        }

        public async Task DeleteProduct(string productCode)
        {
            try
            {
                await _unitOfWork.ProductRepository.Delete(productCode);
            }
            catch (KeyNotFoundException)
            {
                throw new AppException(ErrorCode.PRODUCT_NOT_FOUND);
            }
        }
    }
}
