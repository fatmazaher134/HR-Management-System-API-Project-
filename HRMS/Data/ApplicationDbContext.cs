using HRMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<IdentityUserRole<string>>()
        //       .HasOne<IdentityRole>()
        //       .WithMany()
        //       .HasForeignKey(ur => ur.RoleId)
        //       .OnDelete(DeleteBehavior.NoAction);

                        modelBuilder.Entity<Department>()
                      .HasOne(d => d.Manager)
                      .WithMany() 
                      .HasForeignKey(d => d.ManagerID)
                      .OnDelete(DeleteBehavior.SetNull);
        // cascade loop prevention

        modelBuilder.Entity<Employee>()
                    .HasOne(e => e.Department)
                    .WithMany(d => d.Employees)
                    .HasForeignKey(e => e.DepartmentID)
                    .OnDelete(DeleteBehavior.SetNull);


            modelBuilder.Entity<LeaveRequest>()
        .HasOne(l => l.Employee)
        .WithMany(e => e.LeaveRequests)
        .HasForeignKey(l => l.EmployeeID)
        .OnDelete(DeleteBehavior.Restrict);




        modelBuilder.Entity<SalaryComponent>().HasData(
                  new SalaryComponent
                  {
                      ComponentID  = 1,
                      ComponentName = "Basic Salary",
                      ComponentType = ComponentType.Allowance
                  }
                  );





        //modelBuilder.Entity<LeaveRequest>()
        //        .HasOne(l => l.ApprovedByHR)
        //        .WithMany(e => e.LeaveRequests)
        //        .HasForeignKey(l => l.ApprovedByHRID)
        //        .OnDelete(DeleteBehavior.Restrict);
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ApplicationUser>()
        .HasOne(a => a.Employee)          // ApplicationUser has one Employee
        .WithOne(e => e.ApplicationUser) // Employee has one ApplicationUser
        .HasForeignKey<Employee>(e => e.ApplicationUserId) // The foreign key is Employee.ApplicationUserId
        .OnDelete(DeleteBehavior.Cascade); 

    }

    public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeDocument> EmployeeDocuments { get; set; }
    public DbSet<JobTitle> JobTitles { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }
    public DbSet<LeaveType> LeaveTypes { get; set; }
    public DbSet<Payslip> Payslips { get; set; }
    public DbSet<PayslipDetail> payslipDetails { get; set; }

}
