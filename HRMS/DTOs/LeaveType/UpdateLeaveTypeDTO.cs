namespace HRMS.DTOs.LeaveType
{
    public class UpdateLeaveTypeDTO
    {
        public int LeaveTypeID { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public int DefaultBalance { get; set; }
    }
}
