using System;
using System.Collections.Generic;
using Cli.Params.Suppressors;
using Lib.Suppressors;

namespace Cli.Params
{
    internal static class SuppressorFactory
    {
        internal static IList<ISuppressor> CreateSupressorListFromParams(SuppressorParamsBase[] suppressorParamsList)
        {
            if (suppressorParamsList == null || suppressorParamsList.Length == 0)
            {
                return null;
            }

            var result = new List<ISuppressor>(suppressorParamsList.Length);

            for (var i = 0; i < suppressorParamsList.Length; i++)
            {
                result.Add(CreateSupressorFromParams(suppressorParamsList[i]));
            }

            return result;
        }

        internal static ISuppressor CreateSupressorFromParams(
            SuppressorParamsBase suppressorParams)
        {
            switch (suppressorParams)
            {
                case AdjacentDuplicatesSuppressorParams p:
                    return new AdjacentDuplicatesSuppressor(p.MinLength, p.MaxLength, p.TrackedChars);
                case AdjacentSameCaseSuppressorParams p:
                    return new AdjacentSameCaseSuppressor(p.MinLength, p.MaxLength, p.TrackedChars, p.CharCase);
                case RegexSuppressorParams p:
                    return new RegexSuppressor(p.Pattern);
                default:
                    throw new ArgumentException($"Unsupported suppressor params type: {suppressorParams}.", nameof(suppressorParams));
            }
        }
    }
}
