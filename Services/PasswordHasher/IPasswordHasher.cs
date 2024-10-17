namespace JWT_AUTHENTICATION.Services.PasswordHasher
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password,string hashedPassword);
    }
}
