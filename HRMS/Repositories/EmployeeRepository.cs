using HRMS.Data;
using Microsoft.EntityFrameworkCore;


namespace HRMS.Repositories;

public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public async Task<Employee> UpdateEmployeeAsync(Employee entity)
    {
        var local = _dbSet.Local.FirstOrDefault(e => e.EmployeeID == entity.EmployeeID);
        if (local != null)
        {
            _context.Entry(local).State = EntityState.Detached;
        }

        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return entity;
    }

    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    

    public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync()
    {
        return await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.JobTitle)
            .Where(e => e.IsActive)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task SoftDeleteAsync(int employeeId)
    {
        var employee = await _context.Employees.FindAsync(employeeId);
        if (employee != null)
        {
            employee.IsActive = false;
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }
    }



    public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int departmentId)
    {

        return await _dbSet
            .Where(e => e.DepartmentID == departmentId)
            .ToListAsync();
    }
}

