using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.PartDtos
{
    public class ApprovePartRequestDto
    {
        public int RequestId { get; set; }
        public int ManagerId { get; set; }
        public bool Approve { get; set; } = true;
        public string? Notes { get; set; }
    }
}
