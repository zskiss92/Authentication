using Microsoft.EntityFrameworkCore;
using Page.API.Data;
using Page.API.Models.Authentication;
using System.Security.Claims;

namespace Page.API.Repository.Authentication
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public UserRepository(IHttpContextAccessor contextAccessor, ApplicationDbContext db) : base(db)
        {
            _db = db;
            _contextAccessor = contextAccessor;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
        }

        public async Task<User> GetUserById(string id)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id.ToString().Equals(id));
        }

        public string GetUserEmail()
        {
            return _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        }

        public string GetUserId()
        {
            return _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public async Task<bool> UserExists(string email)
        {
            if (await _db.Users.AnyAsync(u => u.Email.ToLower().Equals(email.ToLower())))
            {
                return true;
            }
            return false;
        }
    }
}
