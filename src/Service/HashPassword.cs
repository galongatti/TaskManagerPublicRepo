using System.Security.Cryptography;
using System.Text;

namespace TaskManagerBackEnd.Service;

public class HashPassword
{
    public static string ComputeHash(string passoword, string salt, string pepper, int interation)
    {
        if (interation <= 0) return passoword;

        using SHA256 sha256 = SHA256.Create();
        string passwordSaltPepper = $"{passoword}{salt}{pepper}";
        byte[] byteValue = Encoding.UTF8.GetBytes(passwordSaltPepper);
        byte[] byteHash = sha256.ComputeHash(byteValue);
        string hash = Convert.ToBase64String(byteHash);
        return ComputeHash(hash, salt, pepper, interation - 1);
    }

    public static string GenerateSalt()
    {
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        byte[] byteSalt = new byte[16];
        rng.GetBytes(byteSalt);
        string salt = Convert.ToBase64String(byteSalt);
        return salt;
    }
}