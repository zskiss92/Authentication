using Page.API.Models.Authentication;
using Page.API.Models.Email;
using Page.API.Repository;
using System.Security.Cryptography;

namespace Page.API.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public AuthService(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<string> Login(string email, string password)
        {
            if(email == string.Empty)
            {
                return "Email is missing";
            }
            
            var user = await _unitOfWork.User.GetUserByEmail(email);

            if ((user == null) || (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)))
            {
                return "User not found or wrong password";
            }
            else if (!user.VerifiedEmail)
            {
                return "Email not verified";
            }

            return "Ok";
        }

        public async Task<bool> Register(User user, string password)
        {
            if(await _unitOfWork.User.UserExists(user.Email))
            {
                return false;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _unitOfWork.User.Add(user);
            _unitOfWork.User.Save();

            await SendVerificationCode(user);

            return true;

        }

        public async Task<bool> SendVerificationCode(User user)
        {
            var emailRequest = new EmailVerification();
            var emailDto = new EmailDto();

            string verificationCode = Guid.NewGuid().ToString("N");

            emailRequest.EmailId = user.Id;
            emailRequest.VerificationKey = verificationCode;

            emailDto.To = user.Email;
            emailDto.Subject = "Verify email address";
            emailDto.Body = "<h1>Please, click on the link below to verify your email address</h1></ br>"
                + "<a href='https://localhost:44306/Auth/" + verificationCode
                + "'>https://localhost:44306/Auth/" + verificationCode + "</a>";

            _emailService.SendEmail(emailDto);


            _unitOfWork.EmailVerification.Add(emailRequest);

            _unitOfWork.EmailVerification.Save();

            return true;
        }

        public async Task<bool> UpdatePassword(string userId, string newPassword)
        {
            var user = await _unitOfWork.User.GetUserById(userId);

            if(user == null)
            {
                return false;
            }

            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _unitOfWork.User.Save();

            return true;
        }

        public async Task<string> VerifyEmail(string key)
        {
            var email = await _unitOfWork.EmailVerification.GetEmailByKey(key); 

            if (email == null)
            {
                return "Verification key not found";
            }

            string emailId = email.EmailId.ToString();

            email.VerificationTime = DateTime.Now;
            _unitOfWork.EmailVerification.Save();

            var user = await _unitOfWork.User.GetUserById(emailId);

            if (user == null)
            {
                return "User not found";
            }

            user.VerifiedEmail = true;
            _unitOfWork.User.Save();

            return "Ok";
        }

        public async Task<string> ForgottenPassword(User user)
        {
            var emailRequest = new ForgottenPassword();
            var emailDto = new EmailDto();

            string verificationCode = Guid.NewGuid().ToString("N");

            emailRequest.EmailId = user.Id;
            emailRequest.VerificationKey = verificationCode;

            emailDto.To = user.Email;
            emailDto.Subject = "Change password";
            emailDto.Body = "<h1>Please, click on the link below to change your password</h1></ br>"
                + "<a href='https://localhost:44306/Auth/verifyPassword/" + verificationCode
                + "'>https://localhost:44306/Auth/verifyPassword/" + verificationCode + "</a>";

            _emailService.SendEmail(emailDto);

            _unitOfWork.ForgottenPassword.Add(emailRequest);

            _unitOfWork.ForgottenPassword.Save();

            return "Ok";
        }

        public async Task<string> VerifyPassword(string key, string newPassword)
        {
            var email = await _unitOfWork.ForgottenPassword.GetEmailByKey(key);

            if (email == null)
            {
                return "Verification key not found";
            }

            if(email.IsPasswordUpdated)
            {
                return "Verification key has already used";
            }

            string emailId = email.EmailId.ToString();

            email.VerificationTime = DateTime.Now;
            email.IsPasswordUpdated = true;
            _unitOfWork.ForgottenPassword.Save();

            var user = await _unitOfWork.User.GetUserById(emailId);

            if (user == null)
            {
                return "User not found";
            }

            string userId = user.Id.ToString();

            await UpdatePassword(userId, newPassword);

            return "Ok";
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash =
                    hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
