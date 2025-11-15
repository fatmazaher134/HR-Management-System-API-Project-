using AutoMapper;
using HRMS.DTOs.LeaveRequest;
using HRMS.Models;

namespace HRMS.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //hear CreateLeaveRequestDto and LeaveRequestDto
            CreateMap<CreateLeaveRequestDto, LeaveRequest>();

            CreateMap<LeaveRequest, LeaveRequestDto>()
                .ForMember(dest => dest.EmployeeName,
                           opt => opt.MapFrom(src =>
                               (src.Employee != null ? src.Employee.FirstName + " " + src.Employee.LastName : string.Empty)))
                .ForMember(dest => dest.LeaveTypeName,
                           opt => opt.MapFrom(src =>
                               (src.LeaveType != null ? src.LeaveType.TypeName : string.Empty)));


        }
    }
}