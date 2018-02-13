using System;
using System.Text;

namespace Lib.PasswordPattern.Extensions
{
    public static class PasswordPatternExtensions
    {
        public static string GetCombinationsString(this PasswordPattern passwordPattern)
        {
            var count = passwordPattern.GetCombinationCount();
            if (count > (ulong) short.MaxValue)
            {
                throw new Exception($"Can't generate too many combinations into memory: {count:N0}");
            }

            var result = new StringBuilder((short) count);
            foreach (var combination in passwordPattern.GetCombinations())
            {
                result.AppendLine(combination);
            }
            return result.ToString();
        }
    }
}
