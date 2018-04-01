using System;
using System.Collections.Generic;
using Cli.Params.VariationGenerators;
using Lib.VariationGenerators;

namespace Cli.Params
{
    internal static class VariationGeneratorFactory
    {
        internal static IList<IVariationGenerator> CreateVariationGeneratorListFromParams(
            VariationGeneratorParamsBase[] variationGeneratorParamsList)
        {
            if (variationGeneratorParamsList == null || variationGeneratorParamsList.Length == 0)
            {
                return null;
            }

            var result = new List<IVariationGenerator>(variationGeneratorParamsList.Length);

            for (var i = 0; i < variationGeneratorParamsList.Length; i++)
            {
                result.Add(CreateVariationGeneratorFromParams(variationGeneratorParamsList[i]));
            }

            return result;
        }

        internal static IVariationGenerator CreateVariationGeneratorFromParams(
            VariationGeneratorParamsBase variationGeneratorParams)
        {
            var charMapper = CharMapperFactory.CreateCharMapper(variationGeneratorParams.CharMapper);
            var supressors = SuppressorFactory.CreateSupressorListFromParams(variationGeneratorParams.SuppressorParams);

            switch (variationGeneratorParams)
            {
                case ArbitraryVariationGeneratorParams p:
                    return new ArbitraryVariationGenerator(p.Chars, p.MinLength, p.MaxLength, p.CharCase, supressors, charMapper);
                case StringListVariationGeneratorParams p:
                    return new StringListVariationGenerator(p.StringList, supressors, charMapper);
                case NumberRangeVariationGeneratorParams p:
                    return new NumberRangeVariationGenerator(p.MinValue, p.MaxValue, p.Step, null, supressors, charMapper);
                case FixedVariationGeneratorParams p:
                    return new FixedVariationGenerator(p.Chars, p.MinLength, p.CharCase, supressors, charMapper);
                case CompoundVariationGeneratorParams p:
                    return new CompoundVariationGenerator(CreateVariationGeneratorListFromParams(p.Generators), supressors, charMapper);
                default:
                    throw new ArgumentException($"Unsupported generator params type: {variationGeneratorParams}.", nameof(variationGeneratorParams));
            }
        }
    }
}
