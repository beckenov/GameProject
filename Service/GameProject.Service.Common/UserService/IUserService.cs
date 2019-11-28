using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GnsGameProject.Service.Common.UserService.Models;

namespace GnsGameProject.Service.Common.UserService
{
    public interface IUserService
    {
        IEnumerable<UserModel> GetUsers();

        Task Remove(Guid id);

        Task<OperationResult> Create(UserModel model);

        Task PrepeareUserForEditAsync(UserModel model);

        Task<UserModel> PrepeareUserForEditView(Guid id);

        Task<UserInfoModel> Details(Guid userId);
    }
}
