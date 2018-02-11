using System.Text;

namespace Lib.CharsetGenerators
{
    public class UpperAndLowerCaseCharsetGenerator : BaseCharsetDecorator
    {
        public UpperAndLowerCaseCharsetGenerator(ICharsetGenerator decoratee) : base(decoratee)
        {
        }

        public override string GenerateCharset(string s)
        {
            s = Decoratee.GenerateCharset(s);

            var collector = new StringBuilder(s.Length * 2);
            foreach (var c in s)
            {
                if (char.IsLetter(c))
                {
                    collector.Append(char.ToLower(c));
                    collector.Append(char.ToUpper(c));
                }
                else
                {
                    collector.Append(c);
                }
            }
            return collector.ToString();
        }
    }
}
