using System.Security.Claims;

namespace JWT_AUTHENTICATION.Models
{
    public class AuthenticationConfiguration
    {
        public string AccessTokenKey { get; set; } = null!;
        public double AccessTokenExpiryMinutes { get; set; }
        public string Audience { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string RefreshTokenKey { get;  set; } 
        public double RefreshTokenExpiryMinutes { get;  set; }
    }
}
