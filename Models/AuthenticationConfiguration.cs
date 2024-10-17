namespace JWT_AUTHENTICATION.Models
{
    public class AuthenticationConfiguration
    {
        public string AccessTokenKey { get; set; }
        public DateTime AccessTokenExpiryMinutes { get; set; }
        public string Audience { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Subject { get; set; } = null!;
    }
}
