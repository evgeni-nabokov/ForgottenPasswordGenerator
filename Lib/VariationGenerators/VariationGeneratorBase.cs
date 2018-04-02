using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Lib.CharMappers;
using Lib.Suppressors;

namespace Lib.VariationGenerators
{
    public abstract class VariationGeneratorBase : IVariationGenerator, ISuppressor
    {
        public string Variation { get; protected set; }

        public ulong VariationNumber { get; protected set; }

        public ulong LoopNumber { get; protected set; }

        public ulong LoopCount { get; protected set; }

        public IReadOnlyList<ISuppressor> Suppressors { get; protected set; }

        public ICharMapper CharMapper { get; }

        protected VariationGeneratorBase(IList<ISuppressor> suppressors, ICharMapper mapper = null)
        {
            if (suppressors != null && suppressors.Count > 0)
            {
                Suppressors = new ReadOnlyCollection<ISuppressor>(suppressors);
                BreaksRestrictionsInternal = BreaksRestrictions;
            }

            CharMapper = mapper;
            if (CharMapper != null)
            {
                MapCharactersInternal = MapCharacters;
            }
        }

        public bool MoveNext()
        {
            while (true)
            {
                if (GoToNextState(out var passedLoops))
                {
                    LoopNumber += passedLoops;
                    var variation = GetCurrentVariation();
                    if (!BreaksRestrictionsInternal(variation))
                    {
                        Variation = variation;
                        VariationNumber++;
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

        }

        public bool BreaksRestrictions(string variation)
        {
            for (var i = 0; i < Suppressors.Count; i++)
            {
                if (Suppressors[i].BreaksRestrictions(variation))
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<string> GetVariations()
        {
            do
            {
                yield return Variation;

            } while (MoveNext());
        }

        public virtual void Dispose()
        {
        }

        public virtual void Reset()
        {
            MoveToFirstVariation();
        }

        protected void MoveToFirstVariation()
        {
            LoopNumber = 1;
            while (true)
            {
                var variation = GetCurrentVariation();
                if (BreaksRestrictionsInternal(variation))
                {
                    if (!GoToNextState(out var passedLoops))
                    {
                        throw new Exception("There are no variations.");
                    }
                    LoopNumber += passedLoops;
                } else {
                    VariationNumber = 1;
                    Variation = variation;
                    break;
                }
            }
        }

        protected abstract bool GoToNextState(out ulong passedLoops);

        protected abstract string BuildVariation();

        protected readonly Func<string, bool> BreaksRestrictionsInternal = variation => false;

        protected readonly Func<string, string> MapCharactersInternal = variation => variation;

        private string GetCurrentVariation()
        {
            return MapCharactersInternal(BuildVariation());
        }

        private string MapCharacters(string variation)
        {
            var result = new StringBuilder(variation);

            for (var i = 0; i < result.Length; i++)
            {
                if (CharMapper.TryGetLetter(variation[i], out var convertedChar))
                {
                    result[i] = convertedChar;
                }
            }
            return result.ToString();
        }

        #region Hidden interface members.

        object IEnumerator.Current => Variation;

        string IEnumerator<string>.Current => Variation;
        
        #endregion
    }
}