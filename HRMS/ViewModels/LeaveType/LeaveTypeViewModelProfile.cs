using AutoMapper;
using HRMS.DTOs.LeaveType;

namespace HRMS.ViewModels.LeaveType
{
    public class LeaveTypeViewModelProfile : Profile
    {
        public LeaveTypeViewModelProfile()
        {
            // ViewModel <-> DTO Mappings

            // LeaveTypeViewModel <-> LeaveTypeDTO
            CreateMap<LeaveTypeViewModel, LeaveTypeDTO>().ReverseMap();

            // LeaveTypeFormViewModel <-> CreateLeaveTypeDTO
            CreateMap<LeaveTypeFormViewModel, CreateLeaveTypeDTO>()
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.TypeName))
                .ForMember(dest => dest.DefaultBalance, opt => opt.MapFrom(src => src.DefaultBalance));

            // LeaveTypeFormViewModel <-> UpdateLeaveTypeDTO
            CreateMap<LeaveTypeFormViewModel, UpdateLeaveTypeDTO>().ReverseMap();
        }
    }
}
