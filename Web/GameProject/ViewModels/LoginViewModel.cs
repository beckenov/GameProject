using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GnsGameProject.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Логин")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
