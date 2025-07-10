using Repository.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Repository.Models.Entities
{
    [Table("user")]
    [Index(nameof(UserCode), IsUnique = true)]
    public class User
    {
        [Key]
        [Column("user_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string UserId { get; set; }

        [Required]
        [Column("user_code")]
        [StringLength(6)]
        public string UserCode { get; set; }

        [Column("role_id")]
        public string RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        [Required]
        [Column("user_name")]
        public string UserName { get; set; }

        [Required]
        [Column("full_name")]
        public string FullName { get; set; }

        [Required]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [Column("password")]
        public string Password { get; set; }

        [Column("warehouse_code")]
        public string WarehouseCode { get; set; }

        [ForeignKey("WarehouseCode")]
        public virtual Warehouse Warehouse { get; set; }

        [Column("status")]
        public UserStatus Status { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public void OnCreate()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public void OnUpdate()
        {
            UpdatedAt = DateTime.Now;
        }
    }
}
