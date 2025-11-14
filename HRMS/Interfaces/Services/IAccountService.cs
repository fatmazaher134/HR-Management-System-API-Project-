using HRMS.ViewModels.Account;
using Microsoft.AspNetCore.Identity;

namespace HRMS.Interfaces.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterViewModel model);

        Task<SignInResult> LoginUserAsync(LoginViewModel model);

        Task LogoutUserAsync();
        List<string> GetAllRoles();
        public Task<IdentityResult> UpdateProfileAsync(ManageAccountViewModel model);


    }
}
