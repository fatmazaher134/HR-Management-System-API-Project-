

namespace HRMS.DTOs.SalaryComponent
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {


            CreateMap<HRMS.Models.SalaryComponent, SalaryComponentDto>();

            CreateMap<CreateSalaryComponentDto, HRMS.Models.SalaryComponent>();

            CreateMap<UpdateSalaryComponentDto, HRMS.Models.SalaryComponent>();
        }
    }
}