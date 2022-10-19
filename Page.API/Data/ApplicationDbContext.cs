using Microsoft.EntityFrameworkCore;
using Page.API.Models.Authentication;

namespace Page.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<EmailVerification> EmailVerifications { get; set; } = null!;
        public DbSet<ForgottenPassword> ForgottenPasswords { get; set; } = null!;
    }
}
