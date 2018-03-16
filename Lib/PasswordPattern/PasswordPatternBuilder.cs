using System;
using System.Collections.Generic;
using Lib.CharMappers;
using Lib.PasswordSections;

namespace Lib.PasswordPattern
{
    public class PasswordPatternBuilder
    {
        private readonly IList<IPasswordSection> _sections;
        private readonly int? _maxSingeCharSequenceLength;
        private readonly int? _maxCapitalLetterSequenceLength;
        private readonly int? _minCapitalLetterDistance;
        private readonly ICharMapper _mapper;


        public PasswordPatternBuilder(
            int? maxSingeCharSequenceLength = null,
            int? maxCapitalLetterSequenceLength = null,
            int? minCapitalLetterDistance = null,
            ICharMapper mapper = null)
        {
            _sections = new List<IPasswordSection>(4);
            _maxSingeCharSequenceLength = maxSingeCharSequenceLength;
            _maxCapitalLetterSequenceLength = maxCapitalLetterSequenceLength;
            _minCapitalLetterDistance = minCapitalLetterDistance;
            _mapper = mapper;
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

        public PasswordPatternBuilder AddArbitraryPasswordSection(
            string chars,
            int maxLength,
            int? minLength = null,
            CharCase charCase = CharCase.AsDefined)
        {
            _sections.Add(new ArbitraryPasswordSection(chars, maxLength, minLength, charCase));
            return this;
        }

        public PasswordPattern Build()
        {
            return new PasswordPattern(
                _sections,
                _maxSingeCharSequenceLength,
                _maxCapitalLetterSequenceLength,
                _minCapitalLetterDistance,
                _mapper
            );
        }
    }
}
