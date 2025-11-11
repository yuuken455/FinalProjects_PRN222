namespace BLL.DTOs.PaymentDtos
{
    public class CreatePaymentDto
    {
        public int AppointmentId { get; set; }

        public int CustomerId { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Incomplete";

        public DateTime? CreatedAt { get; set; }
    }
}
