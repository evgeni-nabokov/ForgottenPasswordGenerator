using System;
using Cli.Params.VariationGenerators;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Cli.Yaml
{
    internal class VariationGeneratorParamsTypeConverter : IYamlTypeConverter
    {
        private static readonly Type BaseNodeType = typeof(VariationGeneratorParamsBase);
        private static readonly Type EndType = typeof(MappingEnd);

        public bool Accepts(Type type)
        {
            return type == BaseNodeType;
        }

        public object ReadYaml(IParser parser, Type type)
        {
            VariationGeneratorParamsBase result;
            parser.Accept<MappingStart>();

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .IgnoreUnmatchedProperties()
                .Build();

            do
            {
                parser.MoveNext();
                var generatorName = YamlParamLoader.GetScalarValue(parser);
                switch (generatorName.ToUpper())
                {
                    case "FIXED":
                        result = deserializer.Deserialize<FixedVariationGeneratorParams>(parser);
                        break;
                    case "STRINGLIST":
                        var stringList = deserializer.Deserialize<string[]>(parser);
                        result = new StringListVariationGeneratorParams { StringList = stringList };
                        break;
                    case "NUMBERRANGE":
                        result = deserializer.Deserialize<NumberRangeVariationGeneratorParams>(parser);
                        break;
                    case "COMPOUND":
                        result = deserializer.Deserialize<CompoundVariationGeneratorParams>(parser);
                        break;
                    case "ARBITRARY":
                        result = deserializer.Deserialize<ArbitraryVariationGeneratorParams>(parser);
                        break;
                    default:
                        throw new YamlException($"Unknown generator: {generatorName}.");
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
