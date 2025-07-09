using System.ComponentModel.DataAnnotations;

namespace Repository.Models.DTO.Request
{
    public class ProductRequest
    {
        [Required(ErrorMessage = "Product code is required")]
        [StringLength(6, ErrorMessage = "Product code must be exactly 6 characters")]
        public string ProductCode { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Size is required")]
        public string Size { get; set; }

        [Required(ErrorMessage = "Color is required")]
        public string Color { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be greater than or equal to 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Product type code is required")]
        public string ProductTypeCode { get; set; } // Relationship with ProductType
    }
}
