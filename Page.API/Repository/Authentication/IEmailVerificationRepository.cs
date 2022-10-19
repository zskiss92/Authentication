using Page.API.Models.Authentication;

namespace Page.API.Repository.Authentication
{
    public interface IEmailVerificationRepository : IRepository<EmailVerification>
    {
        void Save();
        Task<EmailVerification> GetEmailByKey(string key);
    }
}
