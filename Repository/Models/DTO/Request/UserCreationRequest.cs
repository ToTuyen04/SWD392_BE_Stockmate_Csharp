using Repository.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models.DTO.Request
{
    public class UserCreationRequest
    {
        [Required]
        public string UserCode { get; set; }

        [Required]
        public string RoleId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string WarehouseCode { get; set; }

        public UserStatus Status { get; set; } = UserStatus.active;
    }
}
