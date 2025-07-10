using Repository.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Repository.Models.Entities
{
    [Table("noteitem")]
    [Index(nameof(NoteItemCode), IsUnique = true)]
    public class NoteItem
    {
        [Key]
        [Column("noteItem_id")]
        public string NoteItemId { get; set; }

        [Required]
        [Column("noteItem_code")]
        [StringLength(6)]
        public string NoteItemCode { get; set; }

        [Column("product_code")]
        public string ProductCode { get; set; }
        [ForeignKey("ProductCode")]
        public virtual Product Product { get; set; }

        [Column("exchangeNote_id")]
        public string ExchangeNoteId { get; set; }
        [ForeignKey("ExchangeNoteId")]
        public virtual ExchangeNote ExchangeNote { get; set; }

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; }

        [Required]
        [Column("status")]
        public NoteItemStatus Status { get; set; }
    }
}
