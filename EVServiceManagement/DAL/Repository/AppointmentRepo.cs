using DAL.Entities;
using DAL.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class AppointmentRepo : IAppointmentRepo
    {
        private readonly EVServiceManagementContext dbContext;

        public AppointmentRepo(EVServiceManagementContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAppointment(Appointment appointment)
        {
            await dbContext.Appointments.AddAsync(appointment);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Appointment?> GetAppointmentById(int appointmentId)
        {
            return await dbContext.Appointments
                .Include(x => x.Customer)
                .ThenInclude(c => c.Account)
                .Include(appointment => appointment.Vehicle)
                .Include(appointment => appointment.Payment)
                .Include(appointment => appointment.ServiceOrderDetails)
                .ThenInclude(sod => sod.Service)
                .ThenInclude(x => x.ServiceParts)
                .ThenInclude(x => x.Part)
                .Include(appointment => appointment.TechnicianAssignments)
                .ThenInclude(ta => ta.Technician)
                .ThenInclude(t => t.Account)
                .FirstOrDefaultAsync(x => x.AppointmentId == appointmentId);
        }

        public async Task<ICollection<Appointment>> GetAppointments()
        {
            return await dbContext.Appointments
                .Include(x => x.Customer)
                .ThenInclude(c => c.Account)
                .Include(appointment => appointment.Vehicle)
                .Include(appointment => appointment.Payment)
                .Include(appointment => appointment.ServiceOrderDetails)
                .ThenInclude(sod => sod.Service)
                .ThenInclude(x => x.ServiceParts)
                .ThenInclude(x => x.Part)
                .Include(appointment => appointment.TechnicianAssignments)
                .ThenInclude(ta => ta.Technician)
                .ThenInclude(t => t.Account)
                .ToListAsync();
        }

        public async Task<ICollection<Appointment>> GetAppointmentsByCustomerId(int customerId)
        {
            return await dbContext.Appointments.
                Where(appointment => appointment.CustomerId == customerId)
                .Include(x => x.Customer)
                .ThenInclude(c => c.Account)
                .Include(appointment => appointment.Vehicle)
                .Include(appointment => appointment.Payment)
                .Include(appointment => appointment.ServiceOrderDetails)
                .ThenInclude(sod => sod.Service)
                .ThenInclude(x => x.ServiceParts)
                .ThenInclude(x => x.Part)
                .Include(appointment => appointment.TechnicianAssignments)
                .ThenInclude(ta => ta.Technician)
                .ThenInclude(t => t.Account)
                .ToListAsync();
        }

        public async Task<ICollection<Appointment>> GetAppointmentsByTechnician(int technicianId)
        {
            return await dbContext.Appointments
                .Where(a => a.TechnicianAssignments.Any(ta => ta.TechnicianId == technicianId))
                .Include(a => a.Customer).ThenInclude(c => c.Account)
                .Include(a => a.Vehicle)
                .Include(a => a.Payment)
                .Include(a => a.ServiceOrderDetails).ThenInclude(sod => sod.Service).ThenInclude(x => x.ServiceParts)
                .ThenInclude(x => x.Part)
                .Include(a => a.TechnicianAssignments).ThenInclude(ta => ta.Technician).ThenInclude(t => t.Account)
                .ToListAsync();
        }

        public async Task UpdateAppointment(Appointment appointment)
        {
            var existing = await dbContext.Appointments
                .Include(a => a.TechnicianAssignments)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointment.AppointmentId);

            if (existing == null)
            {
                dbContext.Attach(appointment);
                dbContext.Entry(appointment).State = EntityState.Modified;
            }
            else
            {
                existing.Status = appointment.Status;
                existing.Notes = appointment.Notes;
                existing.Date = appointment.Date;
                existing.CustomerId = appointment.CustomerId;
                existing.VehicleId = appointment.VehicleId;

                if (existing.TechnicianAssignments != null && existing.TechnicianAssignments.Count > 0)
                {
                    dbContext.TechnicianAssignments.RemoveRange(existing.TechnicianAssignments.ToList());
                }

                if (appointment.TechnicianAssignments != null)
                {
                    foreach (var ta in appointment.TechnicianAssignments)
                    {
                        existing.TechnicianAssignments.Add(new TechnicianAssignment
                        {
                            AppointmentId = existing.AppointmentId,
                            TechnicianId = ta.TechnicianId,
                            Role = ta.Role,
                            AssignedAt = ta.AssignedAt ?? DateTime.Now
                        });
                    }
                }
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task CompleteAppointmentAsync(int appointmentId, IDictionary<int, int> actualReplaceQuantities, string? note)
        {
            var existing = await dbContext.Appointments
                .Include(a => a.ServiceOrderDetails)
                    .ThenInclude(s => s.Service)
                        .ThenInclude(sp => sp.ServiceParts)
                            .ThenInclude(pp => pp.Part)
                .Include(a => a.Payment) // include payment to check existence
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
            if (existing == null) return;

            foreach (var sod in existing.ServiceOrderDetails)
            {
                if (sod.Service != null && string.Equals(sod.Service.Type, "Replace", StringComparison.OrdinalIgnoreCase))
                {
                    if (actualReplaceQuantities.TryGetValue(sod.OrderDetailId, out var qty))
                    {
                        if (qty < 0) qty = 0;
                        sod.Quantity = qty;

                        // Calculate parts total unit price for this service
                        decimal partsTotalUnit = 0m;
                        if (sod.Service.ServiceParts != null)
                        {
                            partsTotalUnit = sod.Service.ServiceParts.Where(sp => sp.Part != null).Sum(sp => sp.Part.UnitPrice);
                        }
                        // Total = service price + qty * parts price
                        sod.TotalPrice = sod.UnitPrice + (qty * partsTotalUnit);

                        dbContext.Entry(sod).Property(x => x.Quantity).IsModified = true;
                        dbContext.Entry(sod).Property(x => x.TotalPrice).IsModified = true;
                    }
                }
            }

            // Reduce stock for used parts
            foreach (var sod in existing.ServiceOrderDetails.Where(d => d.Service != null && d.Service.Type == "Replace" && d.Quantity > 0))
            {
                var serviceParts = sod.Service?.ServiceParts;
                if (serviceParts == null) continue;
                foreach (var sp in serviceParts)
                {
                    if (sp.Part == null) continue;
                    sp.Part.StockQuantity = Math.Max(0, sp.Part.StockQuantity - sod.Quantity);
                    dbContext.Entry(sp.Part).Property(p => p.StockQuantity).IsModified = true;
                }
            }

            // Recalculate total amount based on updated ServiceOrderDetails
            var totalAmount = existing.ServiceOrderDetails.Sum(s => s.TotalPrice);

            existing.Status = "Completed";
            if (!string.IsNullOrWhiteSpace(note)) existing.Notes = note;
            dbContext.Entry(existing).Property(x => x.Status).IsModified = true;
            dbContext.Entry(existing).Property(x => x.Notes).IsModified = true;

            // Auto create payment if missing
            if (existing.Payment == null)
            {
                var payment = new Payment
                {
                    AppointmentId = existing.AppointmentId,
                    CustomerId = existing.CustomerId,
                    TotalAmount = totalAmount,
                    PaidAmount = 0,
                    RemainingAmount = totalAmount,
                    Status = "UnPaid",
                    CreatedAt = DateTime.Now
                };
                dbContext.Payments.Add(payment);
            }
            else
            {
                // Update existing payment total if already created but not paid
                existing.Payment.TotalAmount = totalAmount;
                existing.Payment.RemainingAmount = (existing.Payment.PaidAmount ?? 0) >= totalAmount ? 0 : totalAmount - (existing.Payment.PaidAmount ?? 0);
                if (string.Equals(existing.Payment.Status, "UnPaid", StringComparison.OrdinalIgnoreCase) || string.Equals(existing.Payment.Status, "Incomplete", StringComparison.OrdinalIgnoreCase))
                {
                    existing.Payment.Status = existing.Payment.RemainingAmount == 0 ? "Paid" : existing.Payment.Status;
                }
                existing.Payment.UpdatedAt = DateTime.Now;
                dbContext.Entry(existing.Payment).State = EntityState.Modified;
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task CancelAppointmentAsync(int appointmentId, string? note)
        {
            var existing = await dbContext.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
            if (existing == null) return;

            existing.Status = "Canceled";
            if (!string.IsNullOrWhiteSpace(note)) existing.Notes = note;

            await dbContext.SaveChangesAsync();
        }

        public async Task<int> AutoMarkInProgressAsync(DateTime nowUtc)
        {
            // assume Date stored local; compare by minute granularity
            var toUpdate = await dbContext.Appointments
                .Where(a => a.Status == "Scheduled" && a.Date <= nowUtc) // window start
                .ToListAsync();
            foreach (var ap in toUpdate)
            {
                ap.Status = "Inprogress"; // unify casing
            }
            if (toUpdate.Count > 0)
            {
                await dbContext.SaveChangesAsync();
            }
            return toUpdate.Count;
        }

        public async Task DeleteAppointmentAsync(int appointmentId)
        {
            var ap = await dbContext.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
            if (ap == null) return;
            dbContext.Appointments.Remove(ap);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeletePendingAppointmentAsync(int appointmentId)
        {
            var ap = await dbContext.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
            if (ap == null) return;
            if (string.Equals(ap.Status, "Pending", StringComparison.OrdinalIgnoreCase))
            {
                dbContext.Appointments.Remove(ap);
                await dbContext.SaveChangesAsync();
            }
        }

        public bool CheckBooking(int vehicleId, DateTime startTime, int duration)
        {
            var endTime = startTime.AddMinutes(duration);

            return dbContext.Appointments.Any(
                  a => (a.Status == "Scheduled" || a.Status == "Inprogress") &&
                       a.VehicleId == vehicleId &&
                       a.Date < endTime &&
                       a.Date.AddMinutes(duration) > startTime);
        }

        public async Task MarkPaymentPaidAsync(int appointmentId)
        {
            var ap = await dbContext.Appointments.Include(a => a.Payment).FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
            if (ap?.Payment == null) return;
            var p = ap.Payment;
            p.PaidAmount = p.TotalAmount;
            p.RemainingAmount = 0;
            p.Status = "Paid";
            p.UpdatedAt = DateTime.Now;
            await dbContext.SaveChangesAsync();
        }
    }
}
