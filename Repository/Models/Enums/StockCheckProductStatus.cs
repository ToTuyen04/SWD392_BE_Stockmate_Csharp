using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models.Enums
{
    public enum StockCheckProductStatus
    {
        temporary,  // Sản phẩm kiểm kho tạm thời, chưa được finalize
        finished,   // Sản phẩm kiểm kho đã hoàn tất
    }
}
