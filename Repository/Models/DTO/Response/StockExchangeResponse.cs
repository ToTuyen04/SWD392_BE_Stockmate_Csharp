using Repository.Models.Enums;

namespace Repository.Models.DTO.Response
{
    public class StockExchangeResponse
    {
        public string TransactionId { get; set; }
        public StockTransactionType TransactionType { get; set; }
        public string SourceWarehouseCode { get; set; }
        public string DestinationWarehouseCode { get; set; }
        public string CreatedBy { get; set; }
        public string ApprovedBy { get; set; }
        public StockExchangeStatus Status { get; set; }
        public List<NoteItemResponse> Items { get; set; }
    }
}
