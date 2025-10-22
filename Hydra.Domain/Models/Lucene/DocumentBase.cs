using System.Security.Cryptography;
using System.Text;

namespace Hydra.Domain.Models.Lucene;

public record DocumentBase(string Id);

public static class Generation
{
    public static string GenerateId(params string[] args)
    {
        using var sha1 = SHA1.Create();
        var combined = string.Join('|', args);
        var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(combined));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}