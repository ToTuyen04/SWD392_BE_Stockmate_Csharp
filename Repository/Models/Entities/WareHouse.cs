using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Repository.Models.Entities
{
    [Table("Warehouse")]
    [Index(nameof(WarehouseCode), IsUnique = true)]
    public class Warehouse
    {
        [Key]
        [Column("warehouse_id")]
        public string WarehouseId { get; set; }

        [Required]
        [Column("warehouse_code")]
        [StringLength(6)]
        public string WarehouseCode { get; set; }

        [Required]
        [Column("warehouse_name")]
        public string WarehouseName { get; set; }

        [Required]
        [Column("address")]
        public string Address { get; set; }
    }
}
