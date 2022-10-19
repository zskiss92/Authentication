using Page.API.Models.Authentication;

namespace Page.API.Repository.Authentication
{
    public interface IUserRepository : IRepository<User>
    {
        void Save();
        string GetUserId();
        string GetUserEmail();
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserById(string id);
        Task<bool> UserExists(string email);
    }
}
