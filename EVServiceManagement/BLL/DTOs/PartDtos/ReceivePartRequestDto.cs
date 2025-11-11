using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.PartDtos
{
    public class ReceivePartRequestDto
    {
        public int RequestId { get; set; }
        public int StaffId { get; set; }
        public bool Received { get; set; } = true;
        public string? Notes { get; set; }
    }
}
