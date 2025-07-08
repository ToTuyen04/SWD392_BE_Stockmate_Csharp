using Repository.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Models.Entities
{
    [Table("StockCheckProduct")]
    public class StockCheckProduct
    {
        [Key]
        [Column("stockCheckProduct_id")]
        public string StockCheckProductId { get; set; }

        [ForeignKey("StockCheckNoteId")]
        public virtual StockCheckNote StockCheckNote { get; set; }

        [ForeignKey("ProductCode")]
        public virtual Product Product { get; set; }

        [Required]
        [Column("last_quantity")]
        public int LastQuantity { get; set; }

        [Required]
        [Column("actual_quantity")]
        public int ActualQuantity { get; set; }

        [Required]
        [Column("total_import_quantity")]
        public int TotalImportQuantity { get; set; } = 0;

        [Required]
        [Column("total_export_quantity")]
        public int TotalExportQuantity { get; set; } = 0;

        [Required]
        [Column("expected_quantity")]
        public int ExpectedQuantity { get; set; }

        [Column("stockCheckProduct_status")]
        public StockCheckProductStatus StockCheckProductStatus { get; set; }

        public void CalculateTheoreticalQuantity()
        {
            ExpectedQuantity = LastQuantity + TotalImportQuantity - TotalExportQuantity;
        }
    }
}
