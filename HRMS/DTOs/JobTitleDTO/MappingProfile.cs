namespace HRMS.DTOs.JobTitleDTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<JobTitle, JobTitleDto>();
            CreateMap<JobTitleFormDto, JobTitle>();

            CreateMap<JobTitleDto, JobTitleViewModel>();
            CreateMap<JobTitleFormViewModel, JobTitleFormDto>();
        }
    }
}
