using System.Text;
using Lib.LetterMappers;

namespace Lib.CharsetGenerators
{
    public class LetterMapCharsetGenerator : BaseCharsetDecorator
    {
        private readonly ILetterMapper _mapper;

        public LetterMapCharsetGenerator(ICharsetGenerator decoratee, ILetterMapper mapper) : base(decoratee)
        {
            _mapper = mapper;
        }

        public override string GenerateCharset(string s)
        {
            s = Decoratee.GenerateCharset(s);

            var collector = new StringBuilder(s.Length);
            foreach (var c in s)
            {
                collector.Append(_mapper.TryGetLetter(c, out var convertedChar) ? convertedChar : c);
            }
            return collector.ToString();
        }
    }
}
