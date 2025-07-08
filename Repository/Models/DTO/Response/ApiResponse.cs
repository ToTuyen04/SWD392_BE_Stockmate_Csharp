using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models.DTO.Response
{
    public class ApiResponse<T>
    {
        public int Code { get; set; } = 1000;
        public string Message { get; set; }
        public T Result { get; set; }
    }
}
