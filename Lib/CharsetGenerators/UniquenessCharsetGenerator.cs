using System.Collections.Generic;
using System.Text;

namespace Lib.CharsetGenerators
{
    public class UniquenessCharsetGenerator : BaseCharsetDecorator
    {
        public UniquenessCharsetGenerator(ICharsetGenerator decoratee) : base(decoratee)
        {
        }

        public override string GenerateCharset(string s)
        {
            s = Decoratee.GenerateCharset(s);

            var uniqueChars = new HashSet<char>();
            var collector = new StringBuilder(s.Length);
            foreach (var c in s)
            {
                if (!uniqueChars.Contains(c))
                {
                    uniqueChars.Add(c);
                    collector.Append(c);
                }
            }
            return collector.ToString();
        }
    }
}
