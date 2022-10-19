using Page.API.Models.Authentication;

namespace Page.API.Services.AuthService
{
    public interface IAuthService
    {
        Task<bool> Register(User user, string password);
        Task<string> Login(string email, string password);
        Task<bool> UpdatePassword(string userId, string newPassword);
        Task<bool> SendVerificationCode(User user);
        Task<string> VerifyEmail(string key);
        Task<string> VerifyPassword(string key, string newPassword);
        Task<string> ForgottenPassword(User user);
    }
}
