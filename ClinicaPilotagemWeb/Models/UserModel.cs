﻿using System.ComponentModel.DataAnnotations;

namespace ClinicaPilotagemWeb.Models
{
    public class UserModel
    {
        [Display(Name = "Email", ResourceType = typeof(Resources.Language))]
        [Required(ErrorMessageResourceType = typeof(Resources.Language),
            ErrorMessageResourceName = "EnterYourEmail")]
        public string Email { get; set; }

        [Display(Name = "Password", ResourceType = typeof(Resources.Language))]
        [Required(ErrorMessageResourceType = typeof(Resources.Language),
            ErrorMessageResourceName = "EnterYourPassword")]
        public string Password { get; set; }
    }
}