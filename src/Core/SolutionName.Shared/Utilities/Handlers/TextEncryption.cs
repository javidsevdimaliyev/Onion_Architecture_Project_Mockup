using System.Security.Cryptography;
using System.Text;

namespace SolutionName.Application.Shared.Utilities.Utility;

public static class TextEncryption
{
    private const string _key = "AAEWAwQFBgcICQoTDA0ODw==";

    private static TripleDESCryptoServiceProvider GetCryproProvider()
    {
        var md5 = new MD5CryptoServiceProvider();

        var key = md5.ComputeHash(Encoding.UTF8.GetBytes(_key));

        return new TripleDESCryptoServiceProvider { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 };
    }

    public static string Encrypt(string plainString)
    {
        var data = Encoding.UTF8.GetBytes(plainString);

        var tripleDes = GetCryproProvider();

        var transform = tripleDes.CreateEncryptor();

        var resultsByteArray = transform.TransformFinalBlock(data, 0, data.Length);

        return Convert.ToBase64String(resultsByteArray).Replace('/', '_');
    }

    public static string Decrypt(string? encryptedString)
    {
        var data = Convert.FromBase64String(encryptedString.Replace('_', '/'));

        var tripleDes = GetCryproProvider();

        var transform = tripleDes.CreateDecryptor();

        var resultsByteArray = transform.TransformFinalBlock(data, 0, data.Length);

        return Encoding.UTF8.GetString(resultsByteArray);
    }

    public static T? Decrypt<T>(string? id)
    {
        if (string.IsNullOrEmpty(id))
            return default;

        return ChangeType<T>(Decrypt(id));
    }

    public static int[] Decrypt(string[] idsHash)
    {
        if (idsHash == null || idsHash.Length == 0)
            return new int[] { };

        var ids = new int[idsHash.Length];

        for (var i = 0; i < ids.Length; i++) ids[i] = int.Parse(Decrypt(idsHash[i]));

        return ids;
    }

    internal static T? ChangeType<T>(object value)
    {
        var t = typeof(T);

        if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
        {
            if (value is null) return default;

            t = Nullable.GetUnderlyingType(t);
        }

        return (T)Convert.ChangeType(value, t);
    }
}