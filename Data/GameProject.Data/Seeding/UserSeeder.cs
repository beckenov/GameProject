using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GnsGameProject.Common;
using GnsGameProject.Data.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GnsGameProject.Data.Seeding
{
    public class UserSeeder
    {
        private const string AdminLogin = "admin";
        private const string Password = "1qaz@WSX";

        public async Task SeedAsync(AppDbContext context, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            await SeedUserAsync(userManager, context);
        }

        private static async Task SeedUserAsync(
            UserManager<User> userManager, AppDbContext context)
        {
            var user = await userManager.FindByNameAsync(AdminLogin);
            if (user == null)
            {
                user = new User()
                {
                    UserName = AdminLogin
                };
                var result = await userManager.CreateAsync(user, Password);
                if (!result.Succeeded)
                {
                    return;
                }
                else
                {
                    var role = await context.Roles.FirstOrDefaultAsync(x => x.Name == GlobalConstants.Roles.Administrator);
                    if (role != null)
                    {
                        context.UserRoles.Add(new IdentityUserRole<Guid>
                        {
                            UserId = user.Id,
                            RoleId = role.Id,
                        });
                    }
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
