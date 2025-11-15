using HRMS.DTOs.SalaryComponent;
using HRMS.Interfaces.Services;
using HRMS.Models;
using HRMS.ViewModels.SalaryComponent;

namespace HRMS.Services.Impelmentation
{
    public class SalaryComponentServices : ISalaryComponentServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper; 

        public SalaryComponentServices(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper; 
        }

        public async Task<IEnumerable<SalaryComponentDto>> GetAllAsync()
        {
            var list = await _unitOfWork.SalaryComponent.GetAllAsync();
            return _mapper.Map<IEnumerable<SalaryComponentDto>>(list);
        }

        public async Task<SalaryComponentDto?> GetByIdAsync(int id)
        {
            var c = await _unitOfWork.SalaryComponent.GetByIdAsync(id);
            if (c == null) return null;
            return _mapper.Map<SalaryComponentDto>(c);
        }

        public async Task<SalaryComponentDto> CreateAsync(CreateSalaryComponentDto model)
        {
            var entity = _mapper.Map<SalaryComponent>(model);

            await _unitOfWork.SalaryComponent.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<SalaryComponentDto>(entity); 
        }

        public async Task<bool> UpdateAsync(int id, UpdateSalaryComponentDto model)
        {
            var existingComponent = await _unitOfWork.SalaryComponent.GetByIdAsync(id);
            if (existingComponent == null)
                return false;

            _mapper.Map(model, existingComponent); 

            await _unitOfWork.SalaryComponent.UpdateAsync(existingComponent);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _unitOfWork.SalaryComponent.GetByIdAsync(id);
            if (existing == null) return false;

            await _unitOfWork.SalaryComponent.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
