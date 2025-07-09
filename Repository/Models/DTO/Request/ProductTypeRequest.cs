using System.ComponentModel.DataAnnotations;

namespace Repository.Models.DTO.Request
{
    public class ProductTypeRequest
    {
        [Required(ErrorMessage = "Product type code is required")]
        public string ProductTypeCode { get; set; }

        [Required(ErrorMessage = "Product type name is required")]
        public string ProductTypeName { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Category code is required")]
        public string CategoryCode { get; set; }
    }
}
