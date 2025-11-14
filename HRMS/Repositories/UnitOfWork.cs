using HRMS.Data;
using HRMS.Interfaces;
using HRMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction _transaction;


    public IApplicationUserRepository ApplicationUser { get; }
    public IAttendanceRecordRepository AttendanceRecord { get; }
    public IDepartmentRepository Department { get; }
    public IEmployeeDocumentRepository EmployeeDocument { get; }
    public IEmployeeRepository Employee { get; }
    public IJobTitleRepository JobTitle { get; }
    public ILeaveRequestRepository LeaveRequest { get; }
    public ILeaveTypeRepository LeaveType { get; }
    public IPayslipDetailRepository PayslipDetail { get; }
    public IPayslipRepository Payslip { get; }
    public ISalaryComponentRepository SalaryComponent { get; }

    public UnitOfWork(
        ApplicationDbContext context,
        IApplicationUserRepository applicationUserRepository,
        IAttendanceRecordRepository attendanceRecordRepository,
        IDepartmentRepository departmentRepository,
        IEmployeeDocumentRepository employeeDocumentRepository,
        IEmployeeRepository employeeRepository,
        IJobTitleRepository jobTitleRepository,
        ILeaveRequestRepository leaveRequestRepository,
        ILeaveTypeRepository leaveTypeRepository,
        IPayslipDetailRepository payslipDetailRepository,
        IPayslipRepository payslipRepository,
        ISalaryComponentRepository salaryComponentRepository
    )
    {
        _context = context;

        ApplicationUser = applicationUserRepository;
        AttendanceRecord = attendanceRecordRepository;
        Department = departmentRepository;
        EmployeeDocument = employeeDocumentRepository;
        Employee = employeeRepository;
        JobTitle = jobTitleRepository;
        LeaveRequest = leaveRequestRepository;
        LeaveType = leaveTypeRepository;
        PayslipDetail = payslipDetailRepository;
        Payslip = payslipRepository;
        SalaryComponent = salaryComponentRepository;
    }

    public async Task<int> SaveChangesAsync()
    {
        foreach (var entry in _context.ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Modified)
            {
                var updatedAtProperty = entry.Properties
                    .FirstOrDefault(p => p.Metadata.Name == "UpdatedAt");

                if (updatedAtProperty is not null)
                    updatedAtProperty.CurrentValue = DateTime.UtcNow;
            }
        }

        return await _context.SaveChangesAsync();
    }
    public IDbContextTransaction BeginTransaction()
    {
       return _transaction = _context.Database.BeginTransaction();
    }

    public void Commit()
    {
        _transaction?.Commit();
        _transaction?.Dispose();
    }

    public void Rollback()
    {
        _transaction?.Rollback();
        _transaction?.Dispose();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}