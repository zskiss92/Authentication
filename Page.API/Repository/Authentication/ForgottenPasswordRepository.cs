using Microsoft.EntityFrameworkCore;
using Page.API.Data;
using Page.API.Models.Authentication;
using System.Linq.Expressions;

namespace Page.API.Repository.Authentication
{
    public class ForgottenPasswordRepository : Repository<ForgottenPassword>, IForgottenPasswordRepository
    {
        private readonly ApplicationDbContext _db;

        public ForgottenPasswordRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ForgottenPassword> GetEmailByKey(string key)
        {
            return await _db.ForgottenPasswords.FirstOrDefaultAsync(f => f.VerificationKey.Equals(key));
        }

        public void Save()
        {
            _db.SaveChanges();
        }

    }
}
