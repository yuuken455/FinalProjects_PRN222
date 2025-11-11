using BLL.DTOs.AccountDtos;
using BLL.DTOs.PaymentDtos;
using BLL.DTOs.ServiceDtos;
using BLL.DTOs.VehicleDtos;

namespace BLL.DTOs.AppointmentDtos
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        public int CustomerId { get; set; }

        public int VehicleId { get; set; }

        public DateTime Date { get; set; }

        public string Status { get; set; } = null!;

        public string? Notes { get; set; }

        public virtual ICollection<PaymentDto> PaymentDtos { get; set; } = new List<PaymentDto>();

        public virtual ICollection<ServiceOrderDetailDto> ServiceOrderDetailDtos { get; set; } = new List<ServiceOrderDetailDto>();

        public virtual ICollection<TechnicianDto> TechnicianDtos { get; set; } = new List<TechnicianDto>();

        public virtual VehicleDto VehicleDto { get; set; } = null!;
    }
}
