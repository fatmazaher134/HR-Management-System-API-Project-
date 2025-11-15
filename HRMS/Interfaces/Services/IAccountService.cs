<<<<<<< HEAD
﻿// 1. Using namespace is changed from ViewModels to Dtos
using HRMS.Dtos.Account;
=======
﻿using HRMS.ViewModels.Account;
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
using Microsoft.AspNetCore.Identity;

namespace HRMS.Interfaces.Services
{
    public interface IAccountService
    {
<<<<<<< HEAD
        // 2. Parameter is now RegisterDto
        Task<IdentityResult> RegisterUserAsync(RegisterDto model);

        // 3. Parameter is now LoginDto
        Task<SignInResult> LoginUserAsync(LoginDto model);

        Task LogoutUserAsync();
        List<string> GetAllRoles();
        public Task<IdentityResult> UpdateProfileAsync(string userId, UpdateProfileDto model);

        // 4. Parameter is changed to a new, specific UpdateProfileDto
        //    (See explanation below)
    }
}
=======
        Task<IdentityResult> RegisterUserAsync(RegisterViewModel model);

        Task<SignInResult> LoginUserAsync(LoginViewModel model);

        Task LogoutUserAsync();
        List<string> GetAllRoles();
        public Task<IdentityResult> UpdateProfileAsync(ManageAccountViewModel model);


    }
}
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
