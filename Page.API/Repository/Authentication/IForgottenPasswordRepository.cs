using Page.API.Models.Authentication;

namespace Page.API.Repository.Authentication
{
    public interface IForgottenPasswordRepository : IRepository<ForgottenPassword>
    {
        void Save();
        Task<ForgottenPassword> GetEmailByKey(string key);
    }
}