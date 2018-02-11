using Lib.CharsetGenerators;

namespace Lib.CharsetGenerators
{
    public class UpperCaseCharsetGenerator : BaseCharsetDecorator
    {
        public UpperCaseCharsetGenerator(ICharsetGenerator decoratee) : base(decoratee)
        {
        }

        public override string GenerateCharset(string s)
        {
            return Decoratee.GenerateCharset(s).ToUpper();
        }
    }
}
