using System.ComponentModel.DataAnnotations;

namespace Repository.Models.DTO.Request
{
    public class TransactionItemRequest
    {
        public string NoteItemCode { get; set; }

        [Required(ErrorMessage = "Product code is required")]
        public string ProductCode { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }
    }
}
