using System.IO;
using Cli.Params;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Cli.Yaml
{
    internal class YamlParamLoader
    {
        public ProgramParams Load(string filename)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .WithTypeConverter(new VariationGeneratorParamsTypeConverter())
                .IgnoreUnmatchedProperties()
                .Build();
            return deserializer.Deserialize<ProgramParams>(LoadTextFromFile(filename));
        }

        private string LoadTextFromFile(string filename)
        {
            return File.ReadAllText(filename);
        }


        internal static string GetScalarValue(IParser parser)
        {
            var scalar = parser.Expect<Scalar>();
            return scalar?.Value;
        }
    }
}
