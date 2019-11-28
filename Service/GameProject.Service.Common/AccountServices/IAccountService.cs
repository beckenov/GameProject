using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GnsGameProject.Service.Common.AccountServices
{
    public interface IAccountService
    {
        Task<OperationResult> Login(string userName, string password);

        Task Logout();
    }
}
