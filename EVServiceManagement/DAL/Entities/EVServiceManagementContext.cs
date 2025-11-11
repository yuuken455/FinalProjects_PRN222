using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

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

    public virtual DbSet<ServiceReview> ServiceReviews { get; set; }

    public virtual DbSet<Staff> Staffs { get; set; }

    public virtual DbSet<Technician> Technicians { get; set; }

    public virtual DbSet<TechnicianAssignment> TechnicianAssignments { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Accounts__349DA5A6DAAE28D3");

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
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCC29B5960B3");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Notes).HasMaxLength(500);
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
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D89CFB1060");

            entity.HasIndex(e => e.AccountId, "UQ__Customer__349DA5A74FF34F1B").IsUnique();

            entity.HasOne(d => d.Account).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customers__Accou__3B75D760");
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.ManagerId).HasName("PK__Managers__3BA2AAE182ECE19A");

            entity.HasIndex(e => e.AccountId, "UQ__Managers__349DA5A770CD49BA").IsUnique();

            entity.HasOne(d => d.Account).WithOne(p => p.Manager)
                .HasForeignKey<Manager>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Managers__Accoun__4316F928");
        });

        modelBuilder.Entity<Part>(entity =>
        {
            entity.HasKey(e => e.PartId).HasName("PK__Parts__7C3F0D5032531EE6");

            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Active");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<PartRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__PartRequ__33A8517A3D5A2AF5");

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
                .HasConstraintName("FK__PartReque__Appro__787EE5A0");

            entity.HasOne(d => d.Part).WithMany(p => p.PartRequests)
                .HasForeignKey(d => d.PartId)
                .HasConstraintName("FK__PartReque__PartI__797309D9");

            entity.HasOne(d => d.RequestedByNavigation).WithMany(p => p.PartRequests)
                .HasForeignKey(d => d.RequestedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PartReque__Reque__778AC167");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A3851735D7D");

            entity.HasIndex(e => e.AppointmentId, "UQ__Payments__8ECDFCC39CEBAC2B").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaidAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)");
            entity.Property(e => e.RemainingAmount)
                .HasComputedColumnSql("([TotalAmount]-[PaidAmount])", false)
                .HasColumnType("decimal(13, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Appointment).WithOne(p => p.Payment)
                .HasForeignKey<Payment>(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__Appoin__6C190EBB");

            entity.HasOne(d => d.Customer).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__Custom__6D0D32F4");
        });

        modelBuilder.Entity<PaymentDetail>(entity =>
        {
            entity.HasKey(e => e.PaymentDetailId).HasName("PK__PaymentD__7F4E340F7E93EDD9");

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
                .HasConstraintName("FK__PaymentDe__Payme__72C60C4A");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Services__C51BB00AC567148C");

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
            entity.HasKey(e => e.OrderDetailId).HasName("PK__ServiceO__D3B9D36C4F963B44");

            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Appointment).WithMany(p => p.ServiceOrderDetails)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK__ServiceOr__Appoi__619B8048");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceOrderDetails)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__ServiceOr__Servi__628FA481");
        });

        modelBuilder.Entity<ServicePart>(entity =>
        {
            entity.HasKey(e => e.ServicePartId).HasName("PK__ServiceP__F6D5C6864725D3CA");

            entity.HasOne(d => d.Part).WithMany(p => p.ServiceParts)
                .HasForeignKey(d => d.PartId)
                .HasConstraintName("FK__ServicePa__PartI__5535A963");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceParts)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__ServicePa__Servi__5441852A");
        });

        modelBuilder.Entity<ServiceReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__ServiceR__74BC79CE92910F8C");

            entity.Property(e => e.Comment).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Appointment).WithMany(p => p.ServiceReviews)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServiceRe__Appoi__5CD6CB2B");

            entity.HasOne(d => d.Customer).WithMany(p => p.ServiceReviews)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServiceRe__Custo__5DCAEF64");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staffs__96D4AB17F2E8BB19");

            entity.HasIndex(e => e.AccountId, "UQ__Staffs__349DA5A7B4011EFC").IsUnique();

            entity.HasOne(d => d.Account).WithOne(p => p.Staff)
                .HasForeignKey<Staff>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Staffs__AccountI__3F466844");
        });

        modelBuilder.Entity<Technician>(entity =>
        {
            entity.HasKey(e => e.TechnicianId).HasName("PK__Technici__301F812108E503E9");

            entity.HasIndex(e => e.AccountId, "UQ__Technici__349DA5A7BB54FA2C").IsUnique();

            entity.HasOne(d => d.Account).WithOne(p => p.Technician)
                .HasForeignKey<Technician>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Technicia__Accou__46E78A0C");
        });

        modelBuilder.Entity<TechnicianAssignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId).HasName("PK__Technici__32499E7766783151");

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
                .HasConstraintName("FK__Technicia__Appoi__656C112C");

            entity.HasOne(d => d.Technician).WithMany(p => p.TechnicianAssignments)
                .HasForeignKey(d => d.TechnicianId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Technicia__Techn__66603565");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.VehicleId).HasName("PK__Vehicles__476B5492CA308545");

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
