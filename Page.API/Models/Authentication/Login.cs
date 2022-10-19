using System.ComponentModel.DataAnnotations;

namespace Page.API.Models.Authentication
{
    public class Login
    {
        [Required, EmailAddress, StringLength(50)]
        public string Email { get; set; } = string.Empty;
        [Required, StringLength(25)]
        public string Password { get; set; } = string.Empty;
    }
}
