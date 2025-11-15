using AutoMapper;

namespace HRMS.DTOs.LeaveType
{
    public class LeaveTypeProfile : Profile
    {
        public LeaveTypeProfile()
        {
            // Model <-> DTO Mappings

            // LeaveType Model <-> LeaveTypeDTO
            CreateMap<Models.LeaveType, LeaveTypeDTO>().ReverseMap();

            // CreateLeaveTypeDTO -> LeaveType Model
            CreateMap<CreateLeaveTypeDTO, Models.LeaveType>()
                .ForMember(dest => dest.LeaveTypeID, opt => opt.Ignore())
                .ForMember(dest => dest.LeaveRequests, opt => opt.Ignore());

            // UpdateLeaveTypeDTO -> LeaveType Model
            CreateMap<UpdateLeaveTypeDTO, Models.LeaveType>()
                .ForMember(dest => dest.LeaveRequests, opt => opt.Ignore());
        }
    }
}
