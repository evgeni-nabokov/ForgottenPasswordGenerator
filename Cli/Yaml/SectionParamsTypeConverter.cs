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
        private static readonly Type EndType = typeof(MappingEnd);

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
                switch (sectionName.ToUpper())
                {
                    case "FIXED":
                        result = sectionDeserializer.Deserialize<FixedSectionParams>(parser);
                        break;
                    case "STRINGLIST":
                        var stringList = sectionDeserializer.Deserialize<string[]>(parser);
                        result = new StringListSectionParams { StringList = stringList };
                        break;
                    case "NUMBERRANGE":
                        result = sectionDeserializer.Deserialize<NumberRangeSectionParams>(parser);
                        break;
                    case "COMPOUND":
                        result = sectionDeserializer.Deserialize<CompoundSectionParams>(parser);
                        break;
                    default:
                        result = sectionDeserializer.Deserialize<ArbitrarySectionParams>(parser);
                        break;
                }
            } while (parser.Current.GetType() != EndType);
            parser.MoveNext();
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
