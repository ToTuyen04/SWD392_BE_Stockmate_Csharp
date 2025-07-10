using Repository.Models.Entities;

namespace Repository.Repository.Interface
{
    public interface IWarehouseRepository
    {
        Task<Warehouse> GetByCode(string warehouseCode);
    }
}
