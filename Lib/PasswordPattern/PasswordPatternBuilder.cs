using System;
using System.Collections.Generic;
using Lib.CharMappers;
using Lib.PasswordSections;

namespace Lib.PasswordPattern
{
    public class PasswordPatternBuilder
    {
        private readonly IList<IPasswordSection> _sections;
        private readonly int? _maxSingeCharSequence;
        

        public PasswordPatternBuilder(int? maxSingeCharSequence = null)
        {
            _sections = new List<IPasswordSection>(4);
            _maxSingeCharSequence = maxSingeCharSequence;
        }

        public PasswordPatternBuilder AddSection(IPasswordSection section)
        {
            _sections.Add(section);
            return this;
        }

        public PasswordPatternBuilder AddFixedPasswordSection(
            string chars,
            int? minLength = null,
            CharCase charCase = CharCase.AsDefined,
            ICharMapper mapper = null)
        {
            _sections.Add(new FixedPasswordSection(chars, minLength, charCase, mapper));
            return this;
        }

        public PasswordPatternBuilder AddArbitraryPasswordSection(
            string chars,
            int maxLength,
            int minLength = 0,
            CharCase charCase = CharCase.AsDefined,
            ICharMapper mapper = null)
        {
            _sections.Add(new ArbitraryPasswordSection(chars, maxLength, minLength, charCase, mapper));
            return this;
        }

        public PasswordPattern Build()
        {
            return new PasswordPattern(_sections, _maxSingeCharSequence);
        }
    }
}
