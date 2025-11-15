using HRMS.Models;

namespace HRMS.Interfaces.Services
{
    public interface IJobTitleServices
    {
        Task<IEnumerable<JobTitleDto>> GetAllAsync();
        Task<JobTitleDto?> GetByIdAsync(int id);
        Task<JobTitleDto> AddAsync(JobTitleFormDto dto); 
        Task<bool> UpdateAsync(int id, JobTitleFormDto dto);
        Task<bool> DeleteAsync(int id);
    }

}
