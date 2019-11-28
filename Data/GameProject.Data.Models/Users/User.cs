using System;
using System.Collections.Generic;
using System.Text;
using GnsGameProject.Data.Models.Game;
using Microsoft.AspNetCore.Identity;

namespace GnsGameProject.Data.Models.Users
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; }

        public ICollection<Match> Matches { get; set; }
    }
}
