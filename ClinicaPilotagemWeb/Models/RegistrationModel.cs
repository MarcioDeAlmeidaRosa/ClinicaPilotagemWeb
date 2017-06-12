using System.ComponentModel.DataAnnotations;

namespace ClinicaPilotagemWeb.Models
{
    public class RegistrationModel
    {
        [Display(Name = "Name", ResourceType = typeof(Resources.Language))]
        [Required(ErrorMessageResourceType = typeof(Resources.Language),
            ErrorMessageResourceName = "EnterYourName")]
        public string Name { get; set; }

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