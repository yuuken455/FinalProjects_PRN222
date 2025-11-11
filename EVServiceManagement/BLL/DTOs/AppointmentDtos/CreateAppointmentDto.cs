using BLL.DTOs.ServiceDtos;

namespace BLL.DTOs.AppointmentDtos
{
    public class CreateAppointmentDto
    {
        public int CustomerId { get; set; }

        public int VehicleId { get; set; }

        public DateTime Date { get; set; }

        public string Status { get; set; } = "Pending";

        public string? Notes { get; set; }

        public virtual ICollection<CreateServiceOrderDetail> CreateServiceOrderDetailDtos { get; set; } = new List<CreateServiceOrderDetail>();
    }
}
