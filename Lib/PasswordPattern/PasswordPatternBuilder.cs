using System.Collections.Generic;
using Lib.CharMappers;
using Lib.PasswordPattern.Suppression;
using Lib.PasswordSections;

namespace Lib.PasswordPattern
{
    public class PasswordPatternBuilder
    {
        private readonly IList<IPasswordSection> _sections;

        private ISuppressor _suppressor;
        private ICharMapper _mapper;
        private SuppressOptions _suppressOptions;

        public PasswordPatternBuilder()
        {
            _sections = new List<IPasswordSection>();
        }

        public PasswordPattern Build()
        {
            return new PasswordPattern(
                _sections,
                _suppressor ?? (_suppressOptions != null ? new Suppressor(_suppressOptions) : null),
                _mapper
            );
        }

        public PasswordPatternBuilder SetSuppressor(ISuppressor suppressor)
        {
            _suppressor = suppressor;
            return this;
        }

        public PasswordPatternBuilder SetCharMapper(ICharMapper mapper)
        {
            _mapper = mapper;
            return this;
        }

        public PasswordPatternBuilder SetSuppressOptions(SuppressOptions options)
        {
            _suppressOptions = options;
            return this;
        }

        public PasswordPatternBuilder AddSection(IPasswordSection section)
        {
            _sections.Add(section);
            return this;
        }

        public PasswordPatternBuilder AddFixedPasswordSection(
            string chars,
            int? minLength = null,
            CharCase charCase = CharCase.AsDefined)
        {
            _sections.Add(new FixedPasswordSection(chars, minLength, charCase));
            return this;
        }

        public PasswordPatternBuilder AddStringListPasswordSection(
            string[] stringList)
        {
            _sections.Add(new StringListPasswordSection(stringList));
            return this;
        }

        public PasswordPatternBuilder AddNumberRangePasswordSection(
            int minValue,
            int maxValue,
            int? step = null)
        {
            _sections.Add(new NumberRangePasswordSection(minValue, maxValue, step));
            return this;
        }

        public PasswordPatternBuilder AddCompoundPasswordSection(
            string chars,
            CharCase charCase = CharCase.AsDefined)
        {
            _sections.Add(new CompoundPasswordSection(chars, charCase));
            return this;
        }

        public PasswordPatternBuilder AddArbitraryPasswordSection(
            string chars,
            int maxLength,
            int? minLength = null,
            CharCase charCase = CharCase.AsDefined)
        {
            _sections.Add(new ArbitraryPasswordSection(chars, maxLength, minLength, charCase));
            return this;
        }

        public PasswordPatternBuilder SetForbiddenDuplicateChars(string forbiddenDuplicateChars)
        {
            EnsureOptionsCreated();
            _suppressOptions.ForbiddenDuplicateChars = forbiddenDuplicateChars;
            return this;
        }

        public PasswordPatternBuilder SetAdjacentDuplicateMaxLength(int? adjacentDuplicateMaxLength)
        {
            EnsureOptionsCreated();
            _suppressOptions.AdjacentDuplicateMaxLength = adjacentDuplicateMaxLength;
            return this;
        }

        public PasswordPatternBuilder SetCapitalAdjacentMaxLength(int? capitalAdjacentMaxLength)
        {
            EnsureOptionsCreated();
            _suppressOptions.CapitalAdjacentMaxLength = capitalAdjacentMaxLength;
            return this;
        }

        public PasswordPatternBuilder SetCapitalCharMinDistance(int? capitalCharMinDistance)
        {
            EnsureOptionsCreated();
            _suppressOptions.CapitalCharMinDistance = capitalCharMinDistance;
            return this;
        }

        private void EnsureOptionsCreated()
        {
            if (_suppressOptions == null)
            {
                _suppressOptions = new SuppressOptions();
            }
        }
    }
}
