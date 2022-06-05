using System.Security.Cryptography;

namespace Mela.AuthService.Api.Services;

/// <summary>
///     Класс для шифрования паролей пользователей
/// </summary>
public static class PasswordHashHelper
{
    public static string ComputeHash(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[16];
        rng.GetBytes(salt);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);
        var hash = pbkdf2.GetBytes(20);

        var bytes = new byte[36];
        Array.Copy(salt, 0, bytes, 0, 16);
        Array.Copy(hash, 0, bytes, 16, 20);

        return Convert.ToBase64String(bytes);
    }

    public static bool ValidateHash(string password, string hashedPassword)
    {
        var correctHash = Convert.FromBase64String(hashedPassword);

        byte[] salt = new byte[16];
        Array.Copy(correctHash, 0, salt, 0, 16);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);
        var hash = pbkdf2.GetBytes(20);

        for (int i = 0; i < 20; i++)
            if (hash[i] != correctHash[i + 16])
                return false;

        return true;
    }
}