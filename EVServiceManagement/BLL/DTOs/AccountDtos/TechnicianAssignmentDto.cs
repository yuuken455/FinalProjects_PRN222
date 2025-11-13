using DAL.Entities;

namespace BLL.DTOs.AccountDtos
{
    public class TechnicianAssignmentDto
    {
        public int AssignmentId { get; set; }

        public int AppointmentId { get; set; }

        public int TechnicianId { get; set; }

        public DateTime? AssignedAt { get; set; }

        public string? Role { get; set; }

        public virtual TechnicianDto TechnicianDto { get; set; } = null!;
    }
}
