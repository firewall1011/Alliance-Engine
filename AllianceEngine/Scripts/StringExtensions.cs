using System.Text;

namespace AllianceEngine
{
    public static class StringExtensions
    {
        public static string ToCapital(this string s) => string.IsNullOrEmpty(s) 
            ? s
            : new StringBuilder(s.Length)
                .Append(char.ToUpperInvariant(s[0]))
                .Append(s[1..])
                .ToString();
        
        public static bool StartsWithOptimized(this string s, in string comparator)
        {
            if (comparator.Length > s.Length)
                return false;

            for (int i = 0; i < comparator.Length; i++)
            {
                if (s[i] != comparator[i])
                    return false;
            }

            return true;
        }
    }
}
