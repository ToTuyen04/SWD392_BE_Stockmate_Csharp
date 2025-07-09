using Repository.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Repository.Models.DTO.Request
{
    public class StockExchangeRequest
    {
        public string TransactionId { get; set; }

        [Required(ErrorMessage = "Transaction type is required")]
        public StockTransactionType TransactionType { get; set; }

        public string SourceWarehouseCode { get; set; }

        public string DestinationWarehouseCode { get; set; }

        public string CreatedBy { get; set; }

        public string ApprovedBy { get; set; }

        [Required(ErrorMessage = "Items are required")]
        public List<TransactionItemRequest> Items { get; set; }
    }
}
