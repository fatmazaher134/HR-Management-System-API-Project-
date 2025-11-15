using HRMS.Interfaces.Services;
using HRMS.Models;

namespace HRMS.Services.Impelmentation
{
    public class JobTitleServices : IJobTitleServices
    {
        private readonly IUnitOfWork _unitOfWork;
<<<<<<< HEAD

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
=======
        private readonly IMapper _mapper;

        public JobTitleServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<JobTitleDto>> GetAllAsync()
        {
            var jobTitles = await _unitOfWork.JobTitle.GetAllAsync(); 
            return _mapper.Map<IEnumerable<JobTitleDto>>(jobTitles);
        }

        public async Task<JobTitleDto?> GetByIdAsync(int id)
        {
            var jobTitle = await _unitOfWork.JobTitle.GetByIdAsync(id);
            if (jobTitle == null) return null;
            return _mapper.Map<JobTitleDto>(jobTitle);
        }

        public async Task<JobTitleDto> AddAsync(JobTitleFormDto dto)
        {
            var jobTitle = _mapper.Map<JobTitle>(dto);

            await _unitOfWork.JobTitle.AddAsync(jobTitle);
            await _unitOfWork.SaveChangesAsync(); 

            return _mapper.Map<JobTitleDto>(jobTitle);
        }

        public async Task<bool> UpdateAsync(int id, JobTitleFormDto dto)
        {
            var jobTitleFromDb = await _unitOfWork.JobTitle.GetByIdAsync(id);
            if (jobTitleFromDb == null) return false;

            _mapper.Map(dto, jobTitleFromDb);

            await _unitOfWork.JobTitle.UpdateAsync(jobTitleFromDb);
            await _unitOfWork.SaveChangesAsync();
            return true;
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var jobTitle = await _unitOfWork.JobTitle.GetByIdAsync(id);
<<<<<<< HEAD
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
=======
            if (jobTitle == null) return false;

            await _unitOfWork.JobTitle.DeleteAsync(jobTitle);
            await _unitOfWork.SaveChangesAsync();
            return true;
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
        }
    }
}
