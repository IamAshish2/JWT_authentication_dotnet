using System.ComponentModel.DataAnnotations;

namespace JWT_AUTHENTICATION.Models
{
    public class RefreshToken
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; } = null!;
    }
}
