namespace HRMS.ViewModels.SalaryComponent
{
    public class SalaryComponentViewModel
    {
        public int ComponentID { get; set; }
        public string ComponentName { get; set; } = string.Empty;
        public ComponentType ComponentType { get; set; }
    }
}
