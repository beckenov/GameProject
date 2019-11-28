using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GnsGameProject.Data.Models.Users;
using GnsGameProject.Service.Common;
using GnsGameProject.Service.Common.AccountServices;
using Microsoft.AspNetCore.Identity;

namespace GnsGameProject.Service.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> userManager;

        private readonly SignInManager<User> signInManager;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<OperationResult> Login(string userName, string password)
        {
            var result = new OperationResult()
            {
                Succeeded = false,
                Message = "Не удалось войти"
            };
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                result.Message = "Такого пользователя не существует";
            }
            else
            {
                var signInResult = await signInManager.PasswordSignInAsync(user, password, false, false);
                if (signInResult.Succeeded)
                {
                    result.Succeeded = true;
                }
            }

            return result;
        }

        public async Task Logout()
        {
            await this.signInManager.SignOutAsync();
        }
    }
}
