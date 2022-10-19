using Page.API.Repository.Authentication;

namespace Page.API.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository User { get; }
        IEmailVerificationRepository EmailVerification { get; }
        IForgottenPasswordRepository ForgottenPassword { get; }

        void Save();

    }
}
