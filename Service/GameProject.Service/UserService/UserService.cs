using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GnsGameProject.Common;
using GnsGameProject.Data;
using GnsGameProject.Data.Models.Users;
using GnsGameProject.Service.Common;
using GnsGameProject.Service.Common.UserService;
using GnsGameProject.Service.Common.UserService.Models;
using GnsGameProject.Service.Common.WordService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;

namespace GnsGameProject.Service.UserService
{
    public class UserService : IUserService
    {
        private readonly AppDbContext context;
        private readonly UserManager<User> userManager;

        public UserService(AppDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public IEnumerable<UserModel> GetUsers()
        {
            return from user in context.Users
                join userRole in context.UserRoles on user.Id equals userRole.UserId
                join role in context.Roles on userRole.RoleId equals role.Id
                where role.Name == GlobalConstants.Roles.Player
                select new UserModel
                {
                    Id = user.Id,
                    LoginName = user.UserName
                };
        }

        public async Task Remove(Guid id)
        {
            var user = await context.Users.FindAsync(id);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
        }

        public async Task<OperationResult> Create(UserModel model)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == model.LoginName);
            var result = new OperationResult();
            if (user == null)
            {
               user = new User
               {
                   Id = Guid.NewGuid(),
                   UserName = model.LoginName,
                   FullName = model.FullName
               };
               var role = await context.Roles.FirstOrDefaultAsync(x => x.Name == GlobalConstants.Roles.Player);

               context.UserRoles.Add(new IdentityUserRole<Guid>() { UserId = user.Id, RoleId = role.Id});
               var registerResult = await userManager.CreateAsync(user, model.Password);
               if (registerResult.Succeeded)
               {
                   result.Succeeded = true;
               }               
            }
            else
            {
                result.Message = "Игрок с таким логином уже существует";
            }

            return result;
        }

        public async Task PrepeareUserForEditAsync(UserModel model)
        {
            var user = await context.Users.FirstAsync(x => x.Id == model.Id);

            user.UserName = model.LoginName;
            user.FullName = model.FullName;
            
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }

        public async Task<UserModel> PrepeareUserForEditView(Guid id)
        {
            var user = (from u in context.Users
                where u.Id == id
                select new UserModel
                {
                    Id = u.Id,
                    LoginName = u.UserName,
                    FullName = u.FullName
                }).FirstOrDefault();
            return user;
          
        }

        public async Task<UserInfoModel> Details(Guid userId)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("Нет такого пользователя");
            }
            var matches = from u in context.Users
                join match in context.Matches on u.Id equals match.UserId
                join word in context.Words on match.WordId equals word.Id
                where u.Id == userId
                group match by match.Result
                into grp
                select grp;

            var userInfoModel = new UserInfoModel {FullName = user.FullName};
            foreach (var match in matches)
            {
                if (match.Key)
                {
                    userInfoModel.CountOfWin = match.Count();
                }
                else
                {
                    userInfoModel.CountOfLose = match.Count();
                }
            }

            userInfoModel.GameCount = userInfoModel.CountOfWin + userInfoModel.CountOfLose;

            return userInfoModel;
        }
    }
}
