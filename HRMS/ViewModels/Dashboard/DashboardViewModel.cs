namespace HRMS.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        // Employee Specific
        public int LeaveBalance { get; set; }
        public int ApprovedLeavesThisYear { get; set; }

        // Admin/HR Specific
        public int TotalEmployees { get; set; }
        public int PendingLeaveRequests { get; set; }
        public int NewHiresThisMonth { get; set; }
    }
}
