using System.ComponentModel.DataAnnotations;

namespace Projet.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]

        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]

        public string Password { get; set; }
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
