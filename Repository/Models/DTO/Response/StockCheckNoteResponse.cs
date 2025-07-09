namespace Repository.Models.DTO.Response
{
    public class StockCheckNoteResponse
    {
        public string StockCheckNoteId { get; set; }
        public DateTime DateTime { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public string CheckerName { get; set; }
        public string StockCheckStatus { get; set; }
        public List<StockCheckProductResponse> StockCheckProducts { get; set; }
    }
}
