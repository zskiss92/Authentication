
using Page.API.Models.Email;

namespace Page.API.Services.EmailService
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request);
    }
}
