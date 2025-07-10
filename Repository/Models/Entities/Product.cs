using Repository.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Repository.Models.Entities
{
    [Table("product")]
    [Index(nameof(ProductCode), IsUnique = true)]
    public class Product
    {
        [Key]
        [Column("product_id")]
        public string ProductId { get; set; }

        [Required]
        [Column("product_code")]
        [StringLength(6)]
        public string ProductCode { get; set; }

        [Required]
        [Column("product_name")]
        public string ProductName { get; set; }

        [Required]
        [Column("size")]
        public string Size { get; set; }

        [Required]
        [Column("color")]
        public string Color { get; set; }

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("status")]
        public ProductStatus Status { get; set; }

        [Required]
        [Column("productType_code")]
        public string ProductTypeCode { get; set; }

        [ForeignKey("ProductTypeCode")]
        public virtual ProductType? ProductType { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public void OnUpdate()
        {
            UpdatedAt = DateTime.Now;
        }
    }
}
