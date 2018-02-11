namespace Lib.CharsetGenerators
{
    public class LowerCaseCharsetGenerator : BaseCharsetDecorator
    {
        public LowerCaseCharsetGenerator(ICharsetGenerator decoratee) : base(decoratee)
        {
        }

        public override string GenerateCharset(string s)
        {
            return Decoratee.GenerateCharset(s).ToLower();
        }
    }
}
