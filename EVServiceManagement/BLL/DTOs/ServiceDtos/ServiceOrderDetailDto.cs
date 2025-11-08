using BLL.DTOs.PartDtos;
using DAL.Entities;

namespace BLL.DTOs.ServiceDtos
{
    public class ServiceOrderDetailDto
    {
        public int OrderDetailId { get; set; }

        public int? AppointmentId { get; set; }

        public int? ServiceId { get; set; }

        public int? PartId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public virtual PartDto? PartDto { get; set; }

        public virtual ServiceDto? ServiceDto { get; set; }
    }
}
