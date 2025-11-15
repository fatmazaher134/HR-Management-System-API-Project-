<<<<<<< HEAD
﻿using HRMS.Models;
=======
﻿using HRMS.DTOs.SalaryComponent;
using HRMS.Models;
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
using HRMS.ViewModels.SalaryComponent;

namespace HRMS.Interfaces.Services
{
    public interface ISalaryComponentServices
    {
<<<<<<< HEAD
        Task<IEnumerable<SalaryComponentViewModel>> GetAllAsync();
        Task<SalaryComponentViewModel?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateSalaryComponentViewModel model);
        Task<bool> UpdateAsync(int id, EditSalaryComponentViewModel model);
=======
        Task<IEnumerable<SalaryComponentDto>> GetAllAsync();
        Task<SalaryComponentDto?> GetByIdAsync(int id);
        Task<SalaryComponentDto> CreateAsync(CreateSalaryComponentDto model); 
        Task<bool> UpdateAsync(int id, UpdateSalaryComponentDto model); 
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
        Task<bool> DeleteAsync(int id);
    }

}
