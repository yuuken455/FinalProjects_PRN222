using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.PartDtos
{
    public class UpdatePartDto
    {
        public int PartId { get; set; }
        public string Name { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public string Status { get; set; } = "Active";
    }
}
