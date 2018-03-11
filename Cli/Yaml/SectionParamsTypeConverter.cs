using System;
using Cli.Params;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Cli.Yaml
{
    internal class SectionParamsTypeConverter : IYamlTypeConverter
    {
        private static readonly Type SectionParamsBaseNodeType = typeof(SectionParamsBase);
        private static readonly Type SequenceEndType = typeof(SequenceEnd);

        public bool Accepts(Type type)
        {
            return type == SectionParamsBaseNodeType;
        }

        public object ReadYaml(IParser parser, Type type)
        {
            SectionParamsBase result;
            parser.Accept<MappingStart>();

            var sectionDeserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .IgnoreUnmatchedProperties()
                .Build();

            do
            {
                parser.MoveNext();
                var sectionName = GetScalarValue(parser);
                switch (sectionName)
                {
                    case "fixed":
                        result = sectionDeserializer.Deserialize<FixedSectionParams>(parser);
                        break;
                    default:
                        result = sectionDeserializer.Deserialize<ArbitrarySectionParams>(parser);
                        break;
                }
                parser.MoveNext();
            } while (parser.Current.GetType() != SequenceEndType);

            return result;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            throw new NotImplementedException();
        }

        private string GetScalarValue(IParser parser)
        {
            var scalar = parser.Expect<Scalar>();
            return scalar?.Value;
        }
    }
}
