using Repository.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Models.Entities
{
    [Table("ExchangeNote")]
    public class ExchangeNote
    {
        [Key]
        [Column("exchangeNote_id")]
        public string ExchangeNoteId { get; set; }

        [Required]
        [Column("date")]
        public DateTime Date { get; set; }

        [Required]
        [Column("status")]
        public StockExchangeStatus Status { get; set; }

        [Required]
        [Column("transaction_type")]
        public StockTransactionType TransactionType { get; set; }

        [ForeignKey("SourceWarehouseCode")]
        public virtual Warehouse SourceWarehouse { get; set; }

        [ForeignKey("DestinationWarehouseCode")]
        public virtual Warehouse DestinationWarehouse { get; set; }

        [ForeignKey("CreatedByUserCode")]
        public virtual User CreatedBy { get; set; }

        [ForeignKey("ApprovedByUserCode")]
        public virtual User ApprovedBy { get; set; }

        public virtual ICollection<NoteItem> NoteItems { get; set; }

        [NotMapped]
        public List<NoteItem> TransientNoteItems { get; set; }
    }
}
