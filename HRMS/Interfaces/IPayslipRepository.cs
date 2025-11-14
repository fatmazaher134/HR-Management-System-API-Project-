using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Interfaces;

public interface IPayslipRepository : IGenericRepository<Payslip>
{
    Task<Payslip?> GetPayslipWithDetailsAsync(int payslipId);

    Task<Payslip?> GetMyPayslipWithDetailsAsync(int payslipId, string applicationUserId);
}
