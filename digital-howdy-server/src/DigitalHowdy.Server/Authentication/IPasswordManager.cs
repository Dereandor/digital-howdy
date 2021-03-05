namespace DigitalHowdy.Server.Authentication
{
    public interface IPasswordManager
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}