using HRMS.Data;
using HRMS.Interfaces;
using HRMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Repositories;

public class PayslipRepository : GenericRepository<Payslip>, IPayslipRepository
{
    
    private readonly ApplicationDbContext _context; 

    public PayslipRepository(ApplicationDbContext context) : base(context) 
    {
        _context = context;
    }

    public async Task<Payslip?> GetPayslipWithDetailsAsync(int payslipId)
    {
        return await _context.Payslips
            .Include(p => p.Employee)
            .Include(p => p.PayslipDetails)
                .ThenInclude(pd => pd.SalaryComponent) 
            .AsNoTracking() 
            .FirstOrDefaultAsync(p => p.PayslipID == payslipId);
    }

    public async Task<Payslip?> GetMyPayslipWithDetailsAsync(int payslipId, string applicationUserId)
    {
        return await _context.Payslips
            .Include(p => p.Employee)
            .Include(p => p.PayslipDetails)
                .ThenInclude(pd => pd.SalaryComponent)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PayslipID == payslipId && p.Employee.ApplicationUserId == applicationUserId);
    }
}
