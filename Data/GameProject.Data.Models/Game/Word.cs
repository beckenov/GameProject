using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GnsGameProject.Data.Models.Game
{
    public class Word : BaseModel
    {
        [Required]
        public string Question { get; set; }

        [Required]
        public string SecretWord { get; set; }

        public ICollection<Match> Matches { get; set; }

    }
}
