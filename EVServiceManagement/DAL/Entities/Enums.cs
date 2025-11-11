using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public enum PartRequestStatus
    {
        Pending,    // Staff tạo
        Approved,   // Manager duyệt
        Rejected,   // Manager từ chối
        Received    // Staff xác nhận đã nhận đủ
    }
}
