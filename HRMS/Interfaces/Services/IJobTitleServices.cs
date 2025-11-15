using HRMS.Models;

namespace HRMS.Interfaces.Services
{
    public interface IJobTitleServices
    {
<<<<<<< HEAD
        Task<IEnumerable<JobTitle>> GetAllAsync();
        Task<JobTitle?> GetByIdAsync(int id);
        Task<JobTitle> AddAsync(JobTitle jobTitle);
        Task<bool> UpdateAsync(JobTitle jobTitle);
=======
        Task<IEnumerable<JobTitleDto>> GetAllAsync();
        Task<JobTitleDto?> GetByIdAsync(int id);
        Task<JobTitleDto> AddAsync(JobTitleFormDto dto); 
        Task<bool> UpdateAsync(int id, JobTitleFormDto dto);
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
        Task<bool> DeleteAsync(int id);
    }

}
