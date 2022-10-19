using System.ComponentModel.DataAnnotations;

namespace Page.API.Models.Authentication
{
    public class Register
    {
        [Required, EmailAddress, StringLength(50)]
        public string Email { get; set; } = string.Empty;
        [Required, StringLength(25, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
