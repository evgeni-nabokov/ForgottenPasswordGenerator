using System;
using System.Collections.Generic;
using Cli.Params;
using Lib.Suppressors;

namespace Cli.Params
{
    internal static class SuppressorFactory
    {
        internal static IList<ISuppressor> CreateSupressorListFromParams(SuppressorParams[] suppressorParamsList)
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
            SuppressorParams p)
        {
            switch (p.Type)
            {
                case SuppressorType.AdjacentDuplicates:
                    return new AdjacentDuplicatesSuppressor(p.MinLength, p.MaxLength, p.TrackedChars, p.IgnoreCase);
                case SuppressorType.AdjacentSameCase:
                    return new AdjacentSameCaseSuppressor(p.MinLength, p.MaxLength, p.TrackedChars, p.CharCase);
                case SuppressorType.DuplicatesSpacing:
                    return new DuplicatesSpacingSuppressor(p.MinLength, p.MaxLength, p.TrackedChars, p.IgnoreCase);
                case SuppressorType.SameCaseSpacing:
                    return new SameCaseSpacingSuppressor(p.MinLength, p.MaxLength, p.TrackedChars, p.CharCase);
                case SuppressorType.CharOccurence:
                    return new CharOccurenceSuppressor(p.MinOccurence, p.MaxOccurence, p.TrackedChars, p.IgnoreCase);
                case SuppressorType.Regex:
                    return new RegexSuppressor(p.Pattern);
                default:
                    throw new ArgumentException($"Unsupported suppressor params type: {p}.", nameof(p));
            }
        }
    }
}
