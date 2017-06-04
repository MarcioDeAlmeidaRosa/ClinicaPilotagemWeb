using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicaPilotagemWeb.Models
{
    public class User
    {
        [Display(Name = "Email", ResourceType = typeof(Resources.Language))]
        [Required(ErrorMessageResourceType = typeof(Resources.Language),
            ErrorMessageResourceName = "EnterYourEmail")]
        public String Email { get; set; }

        [Display(Name = "Password", ResourceType = typeof(Resources.Language))]
        [Required(ErrorMessageResourceType = typeof(Resources.Language),
            ErrorMessageResourceName = "EnterYourPassword")]
        public String Password { get; set; }
    }
}