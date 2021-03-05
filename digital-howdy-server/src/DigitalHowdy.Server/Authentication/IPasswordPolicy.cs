namespace DigitalHowdy.Server.Authentication
{
    public interface IPasswordPolicy
    {
        void ApplyPolicy(string password, string confirmPassword);
    }
}