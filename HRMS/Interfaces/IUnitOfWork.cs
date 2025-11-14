using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IApplicationUserRepository ApplicationUser { get; }
    IAttendanceRecordRepository AttendanceRecord { get; }
    IDepartmentRepository Department { get; }
    IEmployeeDocumentRepository EmployeeDocument { get; }
    IEmployeeRepository Employee { get; }
    IJobTitleRepository JobTitle { get; }
    ILeaveRequestRepository LeaveRequest { get; }
    ILeaveTypeRepository LeaveType { get; }
    IPayslipDetailRepository PayslipDetail { get; }
    IPayslipRepository Payslip { get; }
    ISalaryComponentRepository SalaryComponent { get; }

    Task<int> SaveChangesAsync();
    public IDbContextTransaction BeginTransaction();
    public void Commit();

    public void Rollback();

}
