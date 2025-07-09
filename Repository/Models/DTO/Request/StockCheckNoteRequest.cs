using System.ComponentModel.DataAnnotations;

namespace Repository.Models.DTO.Request
{
    public class StockCheckNoteRequest
    {
        [Required(ErrorMessage = "Warehouse code is required")]
        public string WarehouseCode { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Stock check products are required")]
        public List<StockCheckProductRequest> StockCheckProducts { get; set; }
    }
}
