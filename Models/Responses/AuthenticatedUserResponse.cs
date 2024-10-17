namespace JWT_AUTHENTICATION.Models.Responses
{
    public class AuthenticatedUserResponse
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
