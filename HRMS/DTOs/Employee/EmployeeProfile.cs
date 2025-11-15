using AutoMapper;
using HRMS.Models;

namespace HRMS.DTOs.Employee
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            // Model <-> DTO Mappings

            // Employee Model -> EmployeeDTO
            CreateMap<Models.Employee, EmployeeDTO>().ReverseMap();

            // Employee Model -> EmployeeListDTO
            CreateMap<Models.Employee, EmployeeListDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.DepartmentName : "N/A"))
                .ForMember(dest => dest.JobTitleName, opt => opt.MapFrom(src => src.JobTitle != null ? src.JobTitle.TitleName : "N/A"));

            // Employee Model -> EmployeeDetailsDTO
            CreateMap<Models.Employee, EmployeeDetailsDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.DepartmentName : "N/A"))
                .ForMember(dest => dest.JobTitleName, opt => opt.MapFrom(src => src.JobTitle != null ? src.JobTitle.TitleName : "N/A"))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser != null ? src.ApplicationUser.UserName : "N/A"))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => DateTime.Now.Year - src.DateOfBirth.Year))
                .ForMember(dest => dest.YearsOfService, opt => opt.MapFrom(src => DateTime.Now.Year - src.HireDate.Year));

            // CreateEmployeeDTO -> Employee Model
            CreateMap<CreateEmployeeDTO, Models.Employee>()
                .ForMember(dest => dest.EmployeeID, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom(src => src.UserId));

            // UpdateEmployeeDTO -> Employee Model
            CreateMap<UpdateEmployeeDTO, Models.Employee>()
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.ApplicationUserId, opt => opt.Ignore());

            // UpdateEmployeeBasicInfoDTO -> Employee Model
            CreateMap<UpdateEmployeeBasicInfoDTO, Models.Employee>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Employee Model -> MyProfileDTO
            CreateMap<Models.Employee, MyProfileDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.DepartmentName : "N/A"))
                .ForMember(dest => dest.JobTitleName, opt => opt.MapFrom(src => src.JobTitle != null ? src.JobTitle.TitleName : "N/A"))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser != null ? src.ApplicationUser.UserName : "N/A"))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => DateTime.Now.Year - src.DateOfBirth.Year))
                .ForMember(dest => dest.YearsOfService, opt => opt.MapFrom(src => DateTime.Now.Year - src.HireDate.Year))
                .ForMember(dest => dest.TotalLeaveRequests, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovedLeaves, opt => opt.Ignore())
                .ForMember(dest => dest.PendingLeaves, opt => opt.Ignore());
        }
    }
}