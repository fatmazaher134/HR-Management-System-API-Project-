namespace HRMS.DTOs.SalaryComponent
{
    public class SalaryComponentDto
    {
        public int ComponentID { get; set; }
        public string ComponentName { get; set; } = string.Empty;
        public ComponentType ComponentType { get; set; }
    }
}
