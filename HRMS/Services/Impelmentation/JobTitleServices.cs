using HRMS.Interfaces.Services;
using HRMS.Models;

namespace HRMS.Services.Impelmentation
{
    public class JobTitleServices : IJobTitleServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobTitleServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<JobTitle>> GetAllAsync()
        {
            return await _unitOfWork.JobTitle.GetAllAsync();
        }

        public async Task<JobTitle?> GetByIdAsync(int id)
        {
            return await _unitOfWork.JobTitle.GetByIdAsync(id);
        }

        public async Task<JobTitle> AddAsync(JobTitle jobTitle)
        {
            await _unitOfWork.JobTitle.AddAsync(jobTitle);

            await _unitOfWork.SaveChangesAsync();

            return jobTitle;
        }

        public async Task<bool> UpdateAsync(JobTitle jobTitle)
        {
            try
            {
                await _unitOfWork.JobTitle.UpdateAsync(jobTitle);

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
            var jobTitle = await _unitOfWork.JobTitle.GetByIdAsync(id);
            if (jobTitle == null)
            {
                return false;
            }

            try
            {
                await _unitOfWork.JobTitle.DeleteAsync(jobTitle);

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
