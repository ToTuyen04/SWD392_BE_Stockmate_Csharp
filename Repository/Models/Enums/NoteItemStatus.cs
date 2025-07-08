using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models.Enums
{
    public enum NoteItemStatus
    {
        ACTIVE, //sản phẩm trong giao dịch đang hoạt động
        CANCELED, //sản phẩm trong giao dịch bị hủy bỏ
        COMPLETED, //giao dịch của sản phẩm đã hoàn thành
    }
}
