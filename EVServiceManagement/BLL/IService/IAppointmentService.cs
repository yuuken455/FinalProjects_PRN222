using BLL.DTOs.AppointmentDtos;

namespace BLL.IService
{
    public interface IAppointmentService
    {
        Task<ICollection<AppointmentDto>> GetAppointmentsByCustomerIdAsync(int customerId);
        Task CreateAppointment(CreateAppointmentDto createAppointmentDto);
        Task<ICollection<AppointmentDto>> GetAppointments();
        Task<AppointmentDto?> GetAppointmentByIdAsync(int appointmentId);
        Task UpdateAppointment(AppointmentDto appointmentDto);
        Task<ICollection<AppointmentDto>> GetAppointmentsByTechnician(int technicianId);
        Task CompleteAppointmentAsync(int appointmentId, IDictionary<int, int> actualReplaceQuantities, string? note);
        Task CancelAppointmentAsync(int appointmentId, string? note);
        Task<int> AutoMarkInProgressAsync();
        Task DeleteAppointmentAsync(int appointmentId);
        Task DeletePendingAppointmentAsync(int appointmentId);
        Task MarkPaymentPaidAsync(int appointmentId);
    }
}
