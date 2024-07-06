using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Entities;

namespace OutOfOffice.Context;

public partial class OutOfOfficeContext : DbContext
{
    public OutOfOfficeContext()
    {
    }

    public OutOfOfficeContext(DbContextOptions<OutOfOfficeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ApprovalRequest> ApprovalRequests { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<LeaveRequest> LeaveRequests { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApprovalRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("approval_requests_pkey");

            entity.ToTable("approval_requests");

            entity.HasIndex(e => e.LeaveRequestId, "approval_requests_leave_request_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApproverId).HasColumnName("approver_id");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.LeaveRequestId).HasColumnName("leave_request_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(0)
                .HasColumnName("status");

            entity.HasOne(d => d.Approver).WithMany(p => p.ApprovalRequests)
                .HasForeignKey(d => d.ApproverId)
                .HasConstraintName("fk_approval_request_employee");

            entity.HasOne(d => d.LeaveRequest).WithOne(p => p.ApprovalRequest)
                .HasForeignKey<ApprovalRequest>(d => d.LeaveRequestId)
                .HasConstraintName("fk_approval_request_leave_request");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employees_pkey");

            entity.ToTable("employees");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasMaxLength(255)
                .HasColumnName("fullname");
            entity.Property(e => e.OutOfOfficeBalance).HasColumnName("out_of_office_balance");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.PeoplePartnerId).HasColumnName("people_partner_id");
            entity.Property(e => e.Position).HasColumnName("position");
            entity.Property(e => e.Salt)
                .HasMaxLength(255)
                .HasColumnName("salt");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Subdivision).HasColumnName("subdivision");

            entity.HasOne(d => d.PeoplePartner).WithMany(p => p.InversePeoplePartner)
                .HasForeignKey(d => d.PeoplePartnerId)
                .HasConstraintName("fk_people_partner");

            entity.HasMany(d => d.Projects).WithMany(p => p.Employees)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeProject",
                    r => r.HasOne<Project>().WithMany()
                        .HasForeignKey("ProjectId")
                        .HasConstraintName("fk_employee_projects_project"),
                    l => l.HasOne<Employee>().WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_employee_projects_employee"),
                    j =>
                    {
                        j.HasKey("EmployeeId", "ProjectId").HasName("employee_projects_pkey");
                        j.ToTable("employee_projects");
                        j.IndexerProperty<int>("EmployeeId").HasColumnName("employee_id");
                        j.IndexerProperty<int>("ProjectId").HasColumnName("project_id");
                    });
        });

        modelBuilder.Entity<LeaveRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("leave_requests_pkey");

            entity.ToTable("leave_requests");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AbsenseReason).HasColumnName("absense_reason");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Status)
                .HasDefaultValue(0)
                .HasColumnName("status");

            entity.HasOne(d => d.Employee).WithMany(p => p.LeaveRequests)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("fk_leave_request_employee");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("projects_pkey");

            entity.ToTable("projects");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.ProjectType).HasColumnName("project_type");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
