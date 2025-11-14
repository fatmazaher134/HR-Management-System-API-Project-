using HRMS.Interfaces.Services;
using HRMS.Models;
using HRMS.ViewModels.SalaryComponent;

namespace HRMS.Services.Impelmentation
{
    public class SalaryComponentServices : ISalaryComponentServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public SalaryComponentServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SalaryComponentViewModel>> GetAllAsync()
        {
            var list = await _unitOfWork.SalaryComponent.GetAllAsync();
            return list.Select(c => new SalaryComponentViewModel
            {
                ComponentID = c.ComponentID,
                ComponentName = c.ComponentName,
                ComponentType = c.ComponentType
            });
        }

        public async Task<SalaryComponentViewModel?> GetByIdAsync(int id)
        {
            var c = await _unitOfWork.SalaryComponent.GetByIdAsync(id);
            if (c == null) return null;

            return new SalaryComponentViewModel
            {
                ComponentID = c.ComponentID,
                ComponentName = c.ComponentName,
                ComponentType = c.ComponentType
            };
        }

        public async Task<bool> CreateAsync(CreateSalaryComponentViewModel model)
        {
            var entity = new SalaryComponent
            {
                ComponentName = model.ComponentName,
                ComponentType = model.ComponentType
            };

            await _unitOfWork.SalaryComponent.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(int id, EditSalaryComponentViewModel model)
        {
            var existingComponent = await _unitOfWork.SalaryComponent.GetByIdAsync(id);
            if (existingComponent == null)
                return false;

            existingComponent.ComponentName = model.ComponentName;
            existingComponent.ComponentType = model.ComponentType;

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
