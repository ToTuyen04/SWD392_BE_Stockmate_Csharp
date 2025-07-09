using System.ComponentModel.DataAnnotations;

namespace Repository.Models.DTO.Request
{
    public class StockCheckProductRequest
    {
        [Required(ErrorMessage = "Product code is required")]
        public string ProductCode { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Actual quantity must be at least 0")]
        public int ActualQuantity { get; set; }
    }
}
