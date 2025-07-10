using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Repository.Models.Entities
{
    [Table("producttype")]
    [Index(nameof(ProductTypeCode), IsUnique = true)]
    public class ProductType
    {
        [Key]
        [Column("productType_id")]
        public string ProductTypeId { get; set; }

        [Required]
        [Column("productType_code")]
        [StringLength(50)]
        public string ProductTypeCode { get; set; }

        [Required]
        [Column("productType_name")]
        public string ProductTypeName { get; set; }

        [Column("price")]
        public double? Price { get; set; }

        [Required]
        [Column("category_code")]
        public string CategoryCode { get; set; }

        [ForeignKey("CategoryCode")]
        public virtual Category Category { get; set; }
    }
}
