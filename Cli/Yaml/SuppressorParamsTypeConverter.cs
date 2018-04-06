using System;
using Cli.Params;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Cli.Yaml
{
    internal class SuppressorParamsTypeConverter : IYamlTypeConverter
    {
        private static readonly Type BaseNodeType = typeof(SuppressorParams);
        private static readonly Type EndType = typeof(MappingEnd);

        public bool Accepts(Type type)
        {
            return type == BaseNodeType;
        }

        public object ReadYaml(IParser parser, Type type)
        {
            parser.Accept<MappingStart>();
            SuppressorParams result;
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .IgnoreUnmatchedProperties()
                .Build();

            do
            {
                parser.MoveNext();
                var suppressorName = YamlParamLoader.GetScalarValue(parser);
                result = deserializer.Deserialize<SuppressorParams>(parser);
                result.Type = Enum.Parse<SuppressorType>(suppressorName, true);
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
