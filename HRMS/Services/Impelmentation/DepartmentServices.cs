

namespace HRMS.Services.Impelmentation
{
    public class DepartmentServices : IDepartmentServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            var includes = new string[] { "Manager" };

            return await _unitOfWork.Department.FindAllAsync(criteria: null, includes: includes);
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            var includes = new string[] { "Manager" };

            return await _unitOfWork.Department.FindAsync(d => d.DepartmentID == id, includes);
        }

        public async Task<Department> AddAsync(Department department)
        {
            var addedDept = await _unitOfWork.Department.AddAsync(department);

            await _unitOfWork.SaveChangesAsync();

            return addedDept;
        }

        public async Task<bool> UpdateAsync(Department department)
        {
            try
            {
                await _unitOfWork.Department.UpdateAsync(department);

                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var department = await _unitOfWork.Department.GetByIdAsync(id);
            if (department == null)
            {
                return false;
            }

            try
            {
                await _unitOfWork.Department.DeleteAsync(department);

                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}