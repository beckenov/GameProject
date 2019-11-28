using System;
using System.Collections.Generic;
using System.Text;
using GnsGameProject.Data.Models.Users;

namespace GnsGameProject.Data.Models.Game
{
    public class Match : BaseModel
    {
        public Guid UserId { get; set; }

        public Guid WordId { get; set; }

        public bool Result { get; set; }

        public virtual User User { get; set; }

        public virtual Word Word { get; set; }
    }
}
