using System.ComponentModel.DataAnnotations;

namespace Page.API.Models.Authentication
{
    public class UpdatePassword
    {
        [Required, StringLength(25, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
