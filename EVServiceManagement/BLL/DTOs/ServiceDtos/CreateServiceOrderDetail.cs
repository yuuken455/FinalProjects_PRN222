namespace BLL.DTOs.ServiceDtos
{
    public class CreateServiceOrderDetail
    {
        public int? AppointmentId { get; set; }

        public int? ServiceId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
