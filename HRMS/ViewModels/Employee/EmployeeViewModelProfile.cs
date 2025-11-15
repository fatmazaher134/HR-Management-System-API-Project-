using AutoMapper;
using HRMS.DTOs.Employee;

namespace HRMS.ViewModels.Employee
{
    public class EmployeeViewModelProfile : Profile
    {
        public EmployeeViewModelProfile()
        {
            // ViewModel <-> DTO Mappings

            // EmployeeFormViewModel <-> CreateEmployeeDTO
            CreateMap<EmployeeFormViewModel, CreateEmployeeDTO>()
                .ReverseMap();

            // EmployeeFormViewModel <-> UpdateEmployeeDTO
            CreateMap<EmployeeFormViewModel, UpdateEmployeeDTO>()
                .ReverseMap();

            // EmployeeListViewModel <-> EmployeeListDTO
            CreateMap<EmployeeListViewModel, EmployeeListDTO>()
                .ReverseMap();

            // EmployeeDetailsViewModel <-> EmployeeDetailsDTO
            CreateMap<EmployeeDetailsViewModel, EmployeeDetailsDTO>()
                .ReverseMap();

            // EmployeeEditBasicInfoViewModel <-> UpdateEmployeeBasicInfoDTO
            CreateMap<EmployeeEditBasicInfoViewModel, UpdateEmployeeBasicInfoDTO>()
                .ReverseMap();

            // MyProfileViewModel <-> MyProfileDTO
            CreateMap<MyProfileViewModel, MyProfileDTO>()
                .ReverseMap();
        }
    }
}