using System.Collections.Generic;
using Lib.Suppressors;

namespace Lib.VariationGenerators
{
    public interface IVariationGenerator : IEnumerator<string>
    {
        string Variation { get; }
        ulong VariationNumber { get; }
        ulong LoopNumber { get; }
        ulong LoopCount { get; }

        IReadOnlyList<ISuppressor> Suppressors { get; }
        IEnumerable<string> GetVariations();
    }
}
