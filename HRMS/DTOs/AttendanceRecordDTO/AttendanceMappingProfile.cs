using AutoMapper;
using HRMS.Models;

namespace HRMS.DTOs.AttendanceRecordDTO
{
    public class AttendanceMappingProfile : Profile
    {
        public AttendanceMappingProfile()
        {
            // Entity -> DTO
            CreateMap<AttendanceRecord, AttendanceRecordDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src =>
                    src.Employee != null ? $"{src.Employee.FirstName} {src.Employee.LastName}" : null))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src =>
                    src.CheckOutTime.HasValue ? (TimeSpan?)(src.CheckOutTime.Value - src.CheckInTime) : null));

            // Create mapping (used when creating new attendance records)
            CreateMap<AttendanceRecordForCreationDto, AttendanceRecord>()
                .ForMember(dest => dest.AttendanceID, opt => opt.Ignore())
                // allow EmployeeID here for creation
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.CheckInTime.Date))
                .ForMember(dest => dest.CheckInTime, opt => opt.MapFrom(src =>
                    DateTime.SpecifyKind(src.CheckInTime, DateTimeKind.Local).ToUniversalTime()))
                .ForMember(dest => dest.CheckOutTime, opt => opt.MapFrom(src =>
                    src.CheckOutTime.HasValue
                        ? DateTime.SpecifyKind(src.CheckOutTime.Value, DateTimeKind.Local).ToUniversalTime()
                        : (DateTime?)null));

            // Update mapping — use a dedicated Update DTO (nullable fields). Ignore IDs and EmployeeID.
            CreateMap<AttendanceRecordForUpdateDto, AttendanceRecord>()
                .ForMember(dest => dest.AttendanceID, opt => opt.Ignore())
                .ForMember(dest => dest.EmployeeID, opt => opt.Ignore())
                .ForMember(dest => dest.CheckInTime, opt => opt.MapFrom(src =>
                    src.CheckInTime.HasValue
                        ? DateTime.SpecifyKind(src.CheckInTime.Value, DateTimeKind.Local).ToUniversalTime()
                        : (DateTime?)null))
                .ForMember(dest => dest.CheckOutTime, opt => opt.MapFrom(src =>
                    src.CheckOutTime.HasValue
                        ? DateTime.SpecifyKind(src.CheckOutTime.Value, DateTimeKind.Local).ToUniversalTime()
                        : (DateTime?)null))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src =>
                    src.CheckInTime.HasValue ? DateTime.SpecifyKind(src.CheckInTime.Value, DateTimeKind.Local).ToUniversalTime().Date : (DateTime?)null))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
