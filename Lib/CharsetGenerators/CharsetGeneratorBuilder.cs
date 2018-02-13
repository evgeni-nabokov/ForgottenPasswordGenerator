using Lib.CharMappers;

namespace Lib.CharsetGenerators
{
    public class CharsetGeneratorBuilder
    {
        private ICharsetGenerator _charsetGenerator;

        public CharsetGeneratorBuilder()
        {
            _charsetGenerator = new BaseCharsetGenerator();
        }

        public CharsetGeneratorBuilder AddUpperCaseGenerator()
        {
            _charsetGenerator = new UpperCaseCharsetGenerator(_charsetGenerator);
            return this;
        }

        public CharsetGeneratorBuilder AddLowerCaseGenerator()
        {
            _charsetGenerator = new LowerCaseCharsetGenerator(_charsetGenerator);
            return this;
        }

        public CharsetGeneratorBuilder AddUpperAndLowerCaseGenerator()
        {
            _charsetGenerator = new UpperAndLowerCaseCharsetGenerator(_charsetGenerator);
            return this;
        }

        public CharsetGeneratorBuilder AddLetterMapperGenerator(ICharMapper mapper)
        {
            _charsetGenerator = new CharMapperCharsetGenerator(_charsetGenerator, mapper);
            return this;
        }

        public CharsetGeneratorBuilder AddUniquenessCharGenerator()
        {
            _charsetGenerator = new UniquenessCharsetGenerator(_charsetGenerator);
            return this;
        }

        public ICharsetGenerator Build()
        {
            return _charsetGenerator;
        }
    }
}
