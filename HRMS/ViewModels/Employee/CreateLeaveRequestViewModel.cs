using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace HRMS.ViewModels.Employee
{
    public class CreateLeaveRequestViewModel
    {
        [Required]
        public int LeaveTypeID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public string? Comments { get; set; }

        // dropdown to select leave type

        //public IEnumerable<Models.LeaveType>? LeaveTypes { get; set; }

        public IEnumerable<SelectListItem>? LeaveTypes { get; set; }
    }
}
