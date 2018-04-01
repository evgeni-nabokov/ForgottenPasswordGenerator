using System;
using Cli.Params.Suppressors;
using Cli.Params.VariationGenerators;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Cli.Yaml
{
    internal class SuppressorParamsTypeConverter : IYamlTypeConverter
    {
        private static readonly Type BaseNodeType = typeof(SuppressorParamsBase);
        private static readonly Type EndType = typeof(MappingEnd);

        public bool Accepts(Type type)
        {
            return type == BaseNodeType;
        }

        public object ReadYaml(IParser parser, Type type)
        {
            SuppressorParamsBase result;
            parser.Accept<MappingStart>();

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .IgnoreUnmatchedProperties()
                .Build();

            do
            {
                parser.MoveNext();
                var suppressorName = YamlParamLoader.GetScalarValue(parser);
                switch (suppressorName.ToUpper())
                {
                    case "ADJACENTDUPLICATES":
                        result = deserializer.Deserialize<AdjacentDuplicatesSuppressorParams>(parser);
                        break;
                    case "ADJACENTSAMECASE":
                        result = deserializer.Deserialize<AdjacentSameCaseSuppressorParams>(parser);
                        break;
                    case "REGEX":
                        result = deserializer.Deserialize<RegexSuppressorParams>(parser);
                        break;
                    default:
                        throw new YamlException($"Unknown suppressor: {suppressorName}.");
                }
            } while (parser.Current.GetType() != EndType);
            parser.MoveNext();
            return result;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            throw new NotImplementedException();
        }
    }
}
