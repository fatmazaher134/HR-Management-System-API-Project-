namespace HRMS.DTOs.PayrollDTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Payslip, PayslipSummaryDto>()
            .ForMember(dest => dest.EmployeeFullName,
                       opt => opt.MapFrom(src => $"{src.Employee.FirstName} {src.Employee.LastName}"));

            CreateMap<Payslip, PayslipDto>()
                .ForMember(dest => dest.EmployeeName,
                           opt => opt.MapFrom(src => $"{src.Employee.FirstName} {src.Employee.LastName}"))
                .ForMember(dest => dest.PayDate,
                           opt => opt.MapFrom(src => src.GeneratedDate));

            CreateMap<Payslip, PayslipDetailsDto>()
                .ForMember(dest => dest.EmployeeFullName,
                           opt => opt.MapFrom(src => $"{src.Employee.FirstName} {src.Employee.LastName}"))
                .ForMember(dest => dest.EmployeeEmail,
                           opt => opt.MapFrom(src => src.Employee.Email));

            CreateMap<PayslipDetail, PayslipDetailItemDto>()
                .ForMember(dest => dest.ComponentName,
                           opt => opt.MapFrom(src => src.SalaryComponent.ComponentName))
                .ForMember(dest => dest.ComponentType,
                           opt => opt.MapFrom(src => src.SalaryComponent.ComponentType));


            // (DTOs -> ViewModels)
            CreateMap<PayslipSummaryDto, PayslipSummaryViewModel>();
            CreateMap<PayslipDto, PayslipViewModel>();
            CreateMap<PayslipDetailsDto, PayslipDetailsViewModel>();
            CreateMap<PayslipDetailItemDto, PayslipDetailItem>();

            // (ViewModels -> DTOs)
            CreateMap<GeneratePayrollViewModel, GeneratePayrollDto>();
        }
    }
}
