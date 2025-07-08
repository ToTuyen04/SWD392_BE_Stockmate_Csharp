using Repository.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models.DTO.Request
{
    public class UserCreationRequest
    {
        public string UserCode { get; set; }
        public string RoleId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string WarehouseCode { get; set; }
        public UserStatus Status { get; set; }
    }
}
