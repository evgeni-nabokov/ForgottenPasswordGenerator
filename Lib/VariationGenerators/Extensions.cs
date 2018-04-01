using System;
using System.Text;

namespace Lib.VariationGenerators
{
    public static class VariationGeneratorExtensions
    {
        public static string GetVariationsString(this IVariationGenerator variationGenerator)
        {
            if (variationGenerator == null)
            {
                throw new ArgumentNullException(nameof(variationGenerator), "Generator is null");
            }

            if (variationGenerator.LoopCount > ushort.MaxValue)
            {
                throw new Exception($"Can't generate too many variations into memory: {variationGenerator.LoopCount:N0}");
            }

            var result = new StringBuilder((int)variationGenerator.LoopCount);
            foreach (var variation in variationGenerator.GetVariations())
            {
                result.AppendLine(variation);
            }
            return result.ToString();
        }
    }
}
