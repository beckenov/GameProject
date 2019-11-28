using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GnsGameProject.Service.Common.WordService.Models
{
    public class WordModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Заполните поле")]
        public string Question { get; set; }

        [Required(ErrorMessage = "Заполните поле")]
        public string SecretWord { get; set; }

        public int TryCount { get; set; }

        public bool IsLose { get; set; }
    }
}
