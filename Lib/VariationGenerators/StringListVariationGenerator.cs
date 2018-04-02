using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lib.CharMappers;
using Lib.Suppressors;

namespace Lib.VariationGenerators
{
    public sealed class StringListVariationGenerator : VariationGeneratorBase
    {
        public StringListVariationGenerator(
            IList<string> stringList,
            IList<ISuppressor> suppressors = null,
            ICharMapper mapper = null)
            : base(suppressors, mapper)
        {
            StringList = new ReadOnlyCollection<string>(stringList.Distinct().ToList());
            LoopCount = (ulong)StringList.Count;

            Reset();
        }

        public IReadOnlyList<string> StringList { get; }

        protected override bool GoToNextState(out ulong passedLoops)
        {
            if (_currentIndex + 1 < StringList.Count)
            {
                _currentIndex++;
                passedLoops = 1;
                return true;
            }
 
            passedLoops = 0;
            return false;
        }

        protected override string BuildVariation()
        {
            return StringList[_currentIndex];
        }

        public override void Reset()
        {
            _currentIndex = 0;
            base.Reset();
        }

        private int _currentIndex;
    }
}
