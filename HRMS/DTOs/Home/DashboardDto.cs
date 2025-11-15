namespace HRMS.Dtos.Dashboard
{
    // Renamed from DashboardViewModel to DashboardDto, 
    // as "DTO" is the standard convention for API data contracts.
    public class DashboardDto
    {
        // Employee Specific
        public int LeaveBalance { get; set; }
        public int ApprovedLeavesThisYear { get; set; }

        // Admin/HR Specific
        public int TotalEmployees { get; set; }
        public int PendingLeaveRequests { get; set; }
        public int NewHiresThisMonth { get; set; } // This property was unused, but remains part of the DTO
    }
}