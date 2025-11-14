

namespace HRMS.Interfaces;

public interface IEmployeeRepository : IGenericRepository<Employee>
{
    Task<IEnumerable<Employee>> GetActiveEmployeesAsync();
    Task SoftDeleteAsync(int employeeId);

    Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int departmentId);

    public Task<Employee> UpdateEmployeeAsync(Employee entity);


}
