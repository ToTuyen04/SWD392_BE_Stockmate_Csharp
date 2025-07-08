using Repository.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Models.Entities
{
    [Table("StockCheckNote")]
    public class StockCheckNote
    {
        [Key]
        [Column("stockCheckNote_id")]
        public string StockCheckNoteId { get; set; }

        [Required]
        [Column("date_time")]
        public DateTime DateTime { get; set; }

        [ForeignKey("WarehouseCode")]
        public virtual Warehouse Warehouse { get; set; }

        [ForeignKey("CheckerUserCode")]
        public virtual User Checker { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("status")]
        public StockCheckStatus StockCheckStatus { get; set; }

        public virtual ICollection<StockCheckProduct> StockCheckProducts { get; set; }
    }
}
