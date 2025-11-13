using DAL.Entities;

namespace DAL.IRepository
{
    public interface IAppointmentRepo
    {
        Task<ICollection<Appointment>> GetAppointmentsByCustomerId(int customerId);
        Task CreateAppointment(Appointment appointment);
        Task<ICollection<Appointment>> GetAppointments();
        Task<Appointment?> GetAppointmentById(int appointmentId);
        Task<ICollection<Appointment>> GetAppointmentsByTechnician(int technicianId);
        Task UpdateAppointment(Appointment appointment);
        Task CompleteAppointmentAsync(int appointmentId, IDictionary<int, int> actualReplaceQuantities, string? note);
        Task CancelAppointmentAsync(int appointmentId, string? note);
        Task<int> AutoMarkInProgressAsync(DateTime nowUtc);
        Task DeleteAppointmentAsync(int appointmentId);
        Task DeletePendingAppointmentAsync(int appointmentId);
        Task MarkPaymentPaidAsync(int appointmentId);
        bool CheckBooking(int vehicleId, DateTime startTime, int duration);
    }
}
