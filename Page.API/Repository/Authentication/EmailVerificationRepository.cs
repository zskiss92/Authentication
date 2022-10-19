using Microsoft.EntityFrameworkCore;
using Page.API.Data;
using Page.API.Models.Authentication;

namespace Page.API.Repository.Authentication
{
    public class EmailVerificationRepository : Repository<EmailVerification>, IEmailVerificationRepository
    {
        private readonly ApplicationDbContext _db;

        public EmailVerificationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<EmailVerification> GetEmailByKey(string key)
        {
            return await _db.EmailVerifications.FirstOrDefaultAsync(u => u.VerificationKey.Equals(key));
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
