

namespace HRMS.Services.Impelmentation
{
    public class DepartmentServices : IDepartmentServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DepartmentServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
        {
            var departments = await _unitOfWork.Department.GetAllAsync();
            return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
        }

        public async Task<DepartmentDto?> GetByIdAsync(int id)
        {
            var department = await _unitOfWork.Department.GetByIdAsync(id);
            if (department == null) return null;
            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<DepartmentDto> AddAsync(DepartmentFormDto dto)
        {
            var department = _mapper.Map<Department>(dto);

            await _unitOfWork.Department.AddAsync(department);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<bool> UpdateAsync(int id, DepartmentFormDto dto)
        {
            var departmentFromDb = await _unitOfWork.Department.GetByIdAsync(id);
            if (departmentFromDb == null) return false;

            _mapper.Map(dto, departmentFromDb);

            await _unitOfWork.Department.UpdateAsync(departmentFromDb);
            await _unitOfWork.SaveChangesAsync();
            return true;
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

        public async Task<DepartmentDto?> GetByEmpIdAsync(int Empid)
        {
            var EmployeeWithDepartment = await _unitOfWork.Employee.FindAsync(criteria: e => e.EmployeeID == Empid,
                includes: new[] { "Department" }
                );
            var department = EmployeeWithDepartment.Department;
            if (department == null) return null;
            return _mapper.Map<DepartmentDto>(department);
        }
    }
}