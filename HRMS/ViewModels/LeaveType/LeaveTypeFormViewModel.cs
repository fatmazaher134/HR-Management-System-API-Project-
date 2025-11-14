using System.ComponentModel.DataAnnotations;

namespace HRMS.ViewModels.LeaveType
{
    public class LeaveTypeFormViewModel
    {
        public int LeaveTypeID { get; set; }

        [Required(ErrorMessage = "Leave type name is required")]
        [Display(Name = "Leave Type Name")]
        [MaxLength(100)]
        public string TypeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Default balance is required")]
        [Display(Name = "Default Balance (Days per year)")]
        [Range(0, 365, ErrorMessage = "Balance must be between 0 and 365 days")]
        public int DefaultBalance { get; set; }
    }
}
