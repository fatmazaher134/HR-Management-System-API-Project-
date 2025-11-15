using HRMS.Data;
using HRMS.Interfaces;
using HRMS.Interfaces.Services;
using HRMS.Models;
using HRMS.Repositories;
using HRMS.Services.Impelmentation;
using HRMS.Services.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace HRMS.DependencyInjection;

public static class DataAccessServiceRegistration
{
    public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("CS"))
           .LogTo(log => Debug.WriteLine(log), LogLevel.Information)
           .EnableSensitiveDataLogging(true));



        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // -----------------------------
        // 🔹 Repositories
        // -----------------------------
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
        services.AddScoped<IAttendanceRecordRepository, AttendanceRecordRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IEmployeeDocumentRepository, EmployeeDocumentRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IJobTitleRepository, JobTitleRepository>();
        services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
        services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
        services.AddScoped<IPayslipDetailRepository, PayslipDetailRepository>();
        services.AddScoped<IPayslipRepository, PayslipRepository>();
        services.AddScoped<ISalaryComponentRepository, SalaryComponentRepository>();
        services.AddScoped<IPayrollService, PayrollService>();

        // -----------------------------
        // 🔹 Services
        // -----------------------------
        services.AddScoped<IAttendanceRecordServices, AttendanceRecordServices>();
        services.AddScoped<IDepartmentServices, DepartmentServices>();
        services.AddScoped<IEmployeeDocumentServices, EmployeeDocumentServices>();
        services.AddScoped<IEmployeeServices, EmployeeServices>();
        services.AddScoped<IJobTitleServices, JobTitleServices>();
        services.AddScoped<ILeaveRequestServices, LeaveRequestServices>();
        services.AddScoped<ILeaveTypeServices, LeaveTypeServices>();
        services.AddScoped<IPayslipDetailServices, PayslipDetailServices>();
        services.AddScoped<IPayslipServices, PayslipServices>();
        services.AddScoped<ISalaryComponentServices, SalaryComponentServices>();
        services.AddScoped<IAccountService, AccountServic>();
        services.Configure<IdentityOptions>(options => {
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        });

        return services;
    }
}
