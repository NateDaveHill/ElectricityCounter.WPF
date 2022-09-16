using System.Security.Cryptography;
using System.Text;

namespace _65_WPF_Stromzähler;

internal class ComputeStringToSha256Hash
{
    public string ComputeStringToSha256HashMethod(string pwd)
    {
        using (var sha256Hash = SHA256.Create())
        {
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(pwd));

            var stringbuilder = new StringBuilder();
            for (var i = 0; i < bytes.Length; i++)
            {
                stringbuilder.Append(bytes[i].ToString("x2"));
            }

            return stringbuilder.ToString();
        }
    }
}