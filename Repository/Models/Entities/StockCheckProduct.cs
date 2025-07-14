using Repository.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Models.Entities
{
    [Table("stockcheckproduct")]
    public class StockCheckProduct
    {
        [Key]
        [Column("stockCheckProduct_id")]
        public string StockCheckProductId { get; set; }

        [Column("stockCheckNote_id")]
        public string StockCheckNoteId { get; set; }

        [ForeignKey("StockCheckNoteId")]
        public virtual StockCheckNote StockCheckNote { get; set; }

        [Column("product_code")]
        public string ProductCode { get; set; }

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

        [Required]
        [Column("difference")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Difference { get; set; }

        [Column("stockCheckProduct_status")]
        public StockCheckProductStatus StockCheckProductStatus { get; set; }

        public void CalculateTheoreticalQuantity()
        {
            ExpectedQuantity = LastQuantity + TotalImportQuantity - TotalExportQuantity;
            // Difference is computed by database, don't set it manually
        }
    }
}
