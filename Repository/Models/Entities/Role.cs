using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Models.Entities
{
    [Table("role")]
    public class Role
    {
        [Key]
        [Column("role_id")]
        public string RoleId { get; set; }

        [Column("role_type")]
        public string RoleType { get; set; }

        [Required]
        [Column("role_name")]
        public string RoleName { get; set; }
    }
}
