using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Repository.Models.Entities
{
    [Table("Category")]
    [Index(nameof(CategoryCode), IsUnique = true)]
    public class Category
    {
        [Key]
        [Column("category_id")]
        public string CategoryId { get; set; }

        [Required]
        [Column("category_code")]
        [StringLength(50)]
        public string CategoryCode { get; set; }

        [Required]
        [Column("category_name")]
        public string CategoryName { get; set; }
    }
}
