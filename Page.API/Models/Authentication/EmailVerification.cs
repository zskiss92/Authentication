using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Page.API.Models.Authentication
{
    public class EmailVerification
    {
        [Key]
        public int Id { get; set; }
        public Guid EmailId { get; set; }
        [ForeignKey("EmailId")]
        public User? Email { get; set; }
        public string VerificationKey { get; set; } = string.Empty;
        public DateTime KeyCreated { get; set; } = DateTime.Now;
        public DateTime VerificationTime { get; set; }
    }
}
