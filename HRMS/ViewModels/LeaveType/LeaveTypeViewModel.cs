using System.ComponentModel.DataAnnotations;

namespace HRMS.ViewModels.LeaveType
{
    public class LeaveTypeViewModel
    {
        public int LeaveTypeID { get; set; }

        [Display(Name = "Leave Type Name")]
        public string TypeName { get; set; } = string.Empty;

        [Display(Name = "Default Balance (Days)")]
        public int DefaultBalance { get; set; }
    }
}
