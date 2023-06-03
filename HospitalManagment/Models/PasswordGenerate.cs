using System;
using System.Security.Cryptography;
using System.Text;

public static class PasswordGenerate
{
    public static string GenerateRandomPassword(int length)
    {
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-_=+[]{};:,.<>?";

        using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
        {
            var randomBytes = new byte[length];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            var sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                int randomIndex = randomBytes[i] % validChars.Length;
                sb.Append(validChars[randomIndex]);
            }

            return sb.ToString();
        }
    }
}
