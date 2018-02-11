using System.Text;
using Lib.CharMappers;

namespace Lib.CharsetGenerators
{
    public class CharMapperCharsetGenerator : BaseCharsetDecorator
    {
        private readonly ICharMapper _mapper;

        public CharMapperCharsetGenerator(ICharsetGenerator decoratee, ICharMapper mapper) : base(decoratee)
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
