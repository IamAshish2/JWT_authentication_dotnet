using System.ComponentModel.DataAnnotations;

namespace JWT_AUTHENTICATION.Models.Requests
{
    public class RegisterRequest
    {
        [Key]
        public int RegisterId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public string ConformPassword { get; set; } = null!;
    }
}
