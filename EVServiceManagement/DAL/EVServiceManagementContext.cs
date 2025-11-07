using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public partial class EVServiceManagementContext : DbContext
{
    public EVServiceManagementContext()
    {
    }

    public EVServiceManagementContext(DbContextOptions<EVServiceManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<Part> Parts { get; set; }

    public virtual DbSet<PartRequest> PartRequests { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentDetail> PaymentDetails { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceOrderDetail> ServiceOrderDetails { get; set; }

    public virtual DbSet<ServicePart> ServiceParts { get; set; }

    public virtual DbSet<ShiftChangeRequest> ShiftChangeRequests { get; set; }

    public virtual DbSet<Staff> Staffs { get; set; }

    public virtual DbSet<Technician> Technicians { get; set; }

    public virtual DbSet<TechnicianAssignment> TechnicianAssignments { get; set; }

    public virtual DbSet<TechnicianSchedule> TechnicianSchedules { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<WorkShift> WorkShifts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Accounts__349DA5A6F95BD0B5");

            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.Email)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.Password)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("Active");
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCC224451A9F");

            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.PreferredDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Customer).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__Custo__5812160E");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__Vehic__59063A47");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D83A99B24D");

            entity.HasIndex(e => e.AccountId, "UQ__Customer__349DA5A742C3A786").IsUnique();

            entity.HasOne(d => d.Account).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customers__Accou__3B75D760");
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.ManagerId).HasName("PK__Managers__3BA2AAE1E30A7167");

            entity.HasIndex(e => e.AccountId, "UQ__Managers__349DA5A7348AB7A5").IsUnique();

            entity.HasOne(d => d.Account).WithOne(p => p.Manager)
                .HasForeignKey<Manager>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Managers__Accoun__4316F928");
        });

        modelBuilder.Entity<Part>(entity =>
        {
            entity.HasKey(e => e.PartId).HasName("PK__Parts__7C3F0D50CAC84A59");

            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Active");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<PartRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__PartRequ__33A8517AB4A20F26");

            entity.Property(e => e.ApprovalDate).HasColumnType("datetime");
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.PartRequests)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK__PartReque__Appro__75A278F5");

            entity.HasOne(d => d.Part).WithMany(p => p.PartRequests)
                .HasForeignKey(d => d.PartId)
                .HasConstraintName("FK__PartReque__PartI__76969D2E");

            entity.HasOne(d => d.RequestedByNavigation).WithMany(p => p.PartRequests)
                .HasForeignKey(d => d.RequestedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PartReque__Reque__74AE54BC");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A3834424541");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaidAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RemainingAmount)
                .HasComputedColumnSql("([TotalAmount]-[PaidAmount])", false)
                .HasColumnType("decimal(13, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Appointment).WithMany(p => p.Payments)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__Appoin__6754599E");

            entity.HasOne(d => d.Customer).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__Custom__68487DD7");
        });

        modelBuilder.Entity<PaymentDetail>(entity =>
        {
            entity.HasKey(e => e.PaymentDetailId).HasName("PK__PaymentD__7F4E340F5D24743C");

            entity.Property(e => e.Amount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Method)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TransactionCode).HasMaxLength(100);
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Payment).WithMany(p => p.PaymentDetails)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PaymentDe__Payme__6EF57B66");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Services__C51BB00A40D287CC");

            entity.Property(e => e.Km).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Active");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ServiceOrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__ServiceO__D3B9D36C207477E6");

            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Appointment).WithMany(p => p.ServiceOrderDetails)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK__ServiceOr__Appoi__5CD6CB2B");

            entity.HasOne(d => d.Part).WithMany(p => p.ServiceOrderDetails)
                .HasForeignKey(d => d.PartId)
                .HasConstraintName("FK__ServiceOr__PartI__5EBF139D");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceOrderDetails)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__ServiceOr__Servi__5DCAEF64");
        });

        modelBuilder.Entity<ServicePart>(entity =>
        {
            entity.HasKey(e => e.ServicePartId).HasName("PK__ServiceP__F6D5C6860691F08F");

            entity.HasOne(d => d.Part).WithMany(p => p.ServiceParts)
                .HasForeignKey(d => d.PartId)
                .HasConstraintName("FK__ServicePa__PartI__5535A963");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceParts)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__ServicePa__Servi__5441852A");
        });

        modelBuilder.Entity<ShiftChangeRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__ShiftCha__33A8517A69BCD4F8");

            entity.Property(e => e.ApprovedDate).HasColumnType("datetime");
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.Reason).HasMaxLength(500);
            entity.Property(e => e.RequestedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Receiver).WithMany(p => p.ShiftChangeRequestReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ShiftChan__Recei__03F0984C");

            entity.HasOne(d => d.Requester).WithMany(p => p.ShiftChangeRequestRequesters)
                .HasForeignKey(d => d.RequesterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ShiftChan__Reque__02FC7413");

            entity.HasOne(d => d.Schedule).WithMany(p => p.ShiftChangeRequests)
                .HasForeignKey(d => d.ScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ShiftChan__Sched__04E4BC85");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staffs__96D4AB175024BE02");

            entity.HasIndex(e => e.AccountId, "UQ__Staffs__349DA5A7DB82CA61").IsUnique();

            entity.HasOne(d => d.Account).WithOne(p => p.Staff)
                .HasForeignKey<Staff>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Staffs__AccountI__3F466844");
        });

        modelBuilder.Entity<Technician>(entity =>
        {
            entity.HasKey(e => e.TechnicianId).HasName("PK__Technici__301F81212862743F");

            entity.HasIndex(e => e.AccountId, "UQ__Technici__349DA5A75DDCFCF4").IsUnique();

            entity.HasOne(d => d.Account).WithOne(p => p.Technician)
                .HasForeignKey<Technician>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Technicia__Accou__46E78A0C");
        });

        modelBuilder.Entity<TechnicianAssignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId).HasName("PK__Technici__32499E7771C523A1");

            entity.Property(e => e.AssignedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Main");

            entity.HasOne(d => d.Appointment).WithMany(p => p.TechnicianAssignments)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Technicia__Appoi__619B8048");

            entity.HasOne(d => d.Technician).WithMany(p => p.TechnicianAssignments)
                .HasForeignKey(d => d.TechnicianId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Technicia__Techn__628FA481");
        });

        modelBuilder.Entity<TechnicianSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Technici__9C8A5B492550FECC");

            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Scheduled");

            entity.HasOne(d => d.Shift).WithMany(p => p.TechnicianSchedules)
                .HasForeignKey(d => d.ShiftId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Technicia__Shift__7F2BE32F");

            entity.HasOne(d => d.Technician).WithMany(p => p.TechnicianSchedules)
                .HasForeignKey(d => d.TechnicianId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Technicia__Techn__7E37BEF6");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.VehicleId).HasName("PK__Vehicles__476B549281B521F5");

            entity.Property(e => e.CurrentKm).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.LicensePlate)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Model).HasMaxLength(250);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Active");
            entity.Property(e => e.Vin)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("VIN");

            entity.HasOne(d => d.Customer).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vehicles__Custom__4AB81AF0");
        });

        modelBuilder.Entity<WorkShift>(entity =>
        {
            entity.HasKey(e => e.ShiftId).HasName("PK__WorkShif__C0A838811755B6BF");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
