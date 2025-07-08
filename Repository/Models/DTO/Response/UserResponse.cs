using Repository.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models.DTO.Response
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string WarehouseCode { get; set; }
        public Role Role { get; set; }
    }
}
