namespace Repository.Models.DTO.Response
{
    public class StockCheckProductResponse
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int LastQuantity { get; set; }
        public int TotalImportQuantity { get; set; }
        public int TotalExportQuantity { get; set; }
        public int ExpectedQuantity { get; set; }
        public int ActualQuantity { get; set; }
        public int Difference { get; set; }
    }
}
