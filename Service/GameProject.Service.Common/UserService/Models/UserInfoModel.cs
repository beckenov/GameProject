using System;
using System.Collections.Generic;
using System.Text;

namespace GnsGameProject.Service.Common.UserService.Models
{
    public class UserInfoModel
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }
        
        public int CountOfWin { get; set; }

        public int CountOfLose { get; set; }

        public int GameCount { get; set; }
    }
}
