using System.Text.RegularExpressions;

namespace DigitalHowdy.Server.Authentication
{
    public class PasswordPolicy : IPasswordPolicy
    {
        public void ApplyPolicy(string password, string confirmPassword)
        {
            ValidatePassword(password, confirmPassword);
        }

        private void ValidatePassword(string password, string confirmedPassword)
        {
            if (!string.Equals(password, confirmedPassword))
            {
                throw new ValidationErrorException("The password does not match confirmed password");
            }

            var input = password;

            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ValidationErrorException("Password should not be empty");
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,64}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasLowerChar.IsMatch(input))
            {
                throw new ValidationErrorException("Password should contain at least one lower case letter");
            }
            if (!hasUpperChar.IsMatch(input))
            {
                throw new ValidationErrorException("Password should contain at least one upper case letter");
            }

            if (!hasMiniMaxChars.IsMatch(input))
            {
                throw new ValidationErrorException("Password must contain at least 8 characters.");
            }
            if (!hasNumber.IsMatch(input))
            {
                throw new ValidationErrorException("Password should contain at least one numeric value");
            }
        }
    }
}