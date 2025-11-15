

namespace HRMS.DTOs.DepartmentDTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Define your object-object mappings here
            // CreateMap<Source, Destination>();

            CreateMap<Department, DepartmentDto>()
            .ForMember(
                dest => dest.ManagerName,
                opt => opt.MapFrom(src => (src.Manager != null) ? $"{src.Manager.FirstName} {src.Manager.LastName}" : "N/A")
            );

            CreateMap<DepartmentFormDto, Department>();
            CreateMap<DepartmentDto, DepartmentViewModel>();
            CreateMap<DepartmentFormViewModel, DepartmentFormDto>();
        }

    }
}
