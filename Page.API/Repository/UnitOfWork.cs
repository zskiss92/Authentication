using Page.API.Data;
using Page.API.Repository.Authentication;

namespace Page.API.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            User = new UserRepository(_db);
            EmailVerification = new EmailVerificationRepository(_db);
            ForgottenPassword = new ForgottenPasswordRepository(_db);
        }

        public IUserRepository User { get; private set; }
        public IEmailVerificationRepository EmailVerification { get; private set; }
        public IForgottenPasswordRepository ForgottenPassword { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
