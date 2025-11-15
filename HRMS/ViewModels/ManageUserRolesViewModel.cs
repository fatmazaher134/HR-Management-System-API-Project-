namespace HRMS.ViewModels
{
    public class ManageUserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<UserRoleCheckboxViewModel> Roles { get; set; }

        public ManageUserRolesViewModel()
        {
            Roles = new List<UserRoleCheckboxViewModel>();
        }
    }
}
