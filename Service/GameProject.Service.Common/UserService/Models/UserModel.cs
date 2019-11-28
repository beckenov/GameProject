using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GnsGameProject.Service.Common.UserService.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Введите логин")]
        [Display(Name = "Логин")]
        public string LoginName { get; set; }

        [Required(ErrorMessage = "Введите ФИО")]
        [Display(Name = "ФИО")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Введите Пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
