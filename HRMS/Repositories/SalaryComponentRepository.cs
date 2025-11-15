using HRMS.Data;
using HRMS.Interfaces;
using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Repositories;

public class SalaryComponentRepository : GenericRepository<SalaryComponent>, ISalaryComponentRepository
{
    public SalaryComponentRepository(ApplicationDbContext context) : base(context)
    {
    }
}
