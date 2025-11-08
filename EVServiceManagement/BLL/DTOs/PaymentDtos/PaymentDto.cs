namespace BLL.DTOs.PaymentDtos
{
    public class PaymentDto
    {
        public int PaymentId { get; set; }

        public int AppointmentId { get; set; }

        public int CustomerId { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal? PaidAmount { get; set; }

        public decimal? RemainingAmount { get; set; }

        public string PaymentStatus { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<PaymentDetailDto> PaymentDetailDtos { get; set; } = new List<PaymentDetailDto>();
    }
}
