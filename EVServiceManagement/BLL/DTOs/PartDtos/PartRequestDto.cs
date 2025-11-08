using BLL.DTOs.AccountDtos;

namespace BLL.DTOs.PartDtos
{
    public class PartRequestDto
    {
        public int RequestId { get; set; }

        public int RequestedBy { get; set; }

        public int? ApprovedBy { get; set; }

        public int? PartId { get; set; }

        public int Quantity { get; set; }

        public DateTime? RequestDate { get; set; }

        public DateTime? ApprovalDate { get; set; }

        public string Status { get; set; } = null!;

        public string? Notes { get; set; }

        public virtual ManagerDto? ApprovedByNavigation { get; set; }

        public virtual PartDto? PartDto { get; set; }

        public virtual StaffDto RequestedByNavigation { get; set; } = null!;
    }
}
