using HRMS.Interfaces.Services;
using HRMS.Models;

namespace HRMS.Services.Impelmentation
{
    public class JobTitleServices : IJobTitleServices
    {
        private readonly IUnitOfWork _unitOfWork;
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
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var jobTitle = await _unitOfWork.JobTitle.GetByIdAsync(id);
            if (jobTitle == null) return false;

            await _unitOfWork.JobTitle.DeleteAsync(jobTitle);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
