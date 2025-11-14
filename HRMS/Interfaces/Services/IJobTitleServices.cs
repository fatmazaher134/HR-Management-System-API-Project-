using HRMS.Models;

namespace HRMS.Interfaces.Services
{
    public interface IJobTitleServices
    {
        Task<IEnumerable<JobTitle>> GetAllAsync();
        Task<JobTitle?> GetByIdAsync(int id);
        Task<JobTitle> AddAsync(JobTitle jobTitle);
        Task<bool> UpdateAsync(JobTitle jobTitle);
        Task<bool> DeleteAsync(int id);
    }

}
