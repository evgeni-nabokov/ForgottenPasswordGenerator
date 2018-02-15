using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cli.Params;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Cli.Yaml
{
    internal class YamlParamLoader
    {
        public PatternParams Load(string filename)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .WithTypeConverter(new SectionParamsTypeConverter())
                .IgnoreUnmatchedProperties()
                .Build();
            return deserializer.Deserialize<PatternParams>(LoadTextFromFile(filename));
        }

        private string LoadTextFromFile(string filename)
        {
            return File.ReadAllText(filename);
        }
    }
}
