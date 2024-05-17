using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;

namespace Bookstore.Domain.Common;

public static partial class StringExtensions
{
    public static int GetStableHashCode(this string s)
    {
        byte[] title = Encoding.UTF8.GetBytes(s);
        byte[] hashBytes = SHA256.HashData(title);
        return ToInts(hashBytes).Aggregate((a, b) => a ^ b);
    }

    public static IEnumerable<string> SplitIntoWords(this string s) =>
        WordLettersRegex().Matches(s).Select(m => m.Value.ToLowerInvariant());

    private static IEnumerable<int> ToInts(byte[] bytes) =>
        Enumerable.Range(0, bytes.Length / 4).Select(i => BitConverter.ToInt32(bytes, i * 4));

    [GeneratedRegex(@"\p{L}+")] private static partial Regex WordLettersRegex();
}