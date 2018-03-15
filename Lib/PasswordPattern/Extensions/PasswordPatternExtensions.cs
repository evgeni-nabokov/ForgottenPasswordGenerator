using System;
using System.Text;

namespace Lib.PasswordPattern.Extensions
{
    public static class PasswordPatternExtensions
    {
        public static string GetVariationsString(this PasswordPattern passwordPattern)
        {
            var count = passwordPattern.Count;
            if (count > (ulong) short.MaxValue)
            {
                throw new Exception($"Can't generate too many variations into memory: {count:N0}");
            }

            var result = new StringBuilder((short) count);
            foreach (var variation in passwordPattern.GetVariations())
            {
                result.AppendLine(variation);
            }
            return result.ToString();
        }
    }
}
