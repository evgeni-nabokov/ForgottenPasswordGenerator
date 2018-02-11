namespace Lib.CharsetGenerators
{
    public abstract class BaseCharsetDecorator : ICharsetGenerator
    {
        protected readonly ICharsetGenerator Decoratee;

        protected BaseCharsetDecorator(ICharsetGenerator decoratee)
        {
            Decoratee = decoratee;
        }

        public abstract string GenerateCharset(string s);
    }
}
