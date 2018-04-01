using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Lib.CharMappers;
using Lib.Suppressors;

namespace Lib.VariationGenerators
{
    public sealed class CompoundVariationGenerator : VariationGeneratorBase
    {
        private const int MaxVariationLength = 100;

        public CompoundVariationGenerator(
            IList<IVariationGenerator> generators,
            IList<ISuppressor> suppressors = null,
            ICharMapper mapper = null)
            : base(suppressors, mapper)
        {
            if (generators == null || generators.Count == 0)
            {
                throw new ArgumentNullException(nameof(generators), $"Empty parameter: {nameof(generators)}");
            }

            VariationGenerators = new ReadOnlyCollection<IVariationGenerator>(generators);

            LoopCount = GetLoopCount();

            Reset();
        }

        public IReadOnlyList<IVariationGenerator> VariationGenerators { get; }

        private ulong GetLoopCount()
        {
            var result = 1ul;

            for (var i = 0; i < VariationGenerators.Count; i++)
            {
                result *= VariationGenerators[i].LoopCount;
            }

            return result;
        }

        protected override string BuildVariation()
        {
            var result = new StringBuilder(MaxVariationLength);

            for (var i = 0; i < VariationGenerators.Count; i++)
            {
                result.Append(VariationGenerators[i].Variation);
            }

            return result.ToString();
        }

        protected override bool MoveNextInternal(out ulong passedLoops)
        {
            passedLoops = 0;
            while (true)
            {
                var prevLoopNumber = LoopNumber;
                var moved = GoToNextState();
                passedLoops += LoopNumber - prevLoopNumber;
                if (moved)
                {
                    var variation = MapCharactersInternal(BuildVariation());
                    if (!BreaksRestrictionsInternal(variation))
                    {
                        Variation = variation;
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        protected override bool GoToNextState()
        {
            for (var i = 0; i < VariationGenerators.Count; i++)
            {
                var g = VariationGenerators[i];
                var prevLoopNumber = g.LoopNumber;
                if (g.MoveNext())
                {
                    LoopNumber += g.LoopNumber - prevLoopNumber;
                    return true;
                }
                VariationGenerators[i].Reset();
            }

            return false;
        }
    }
}
