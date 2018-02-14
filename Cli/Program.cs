using System;
using System.IO;
using Lib;
using Lib.CharMappers;
using Lib.PasswordPattern;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.Utilities;

namespace Cli
{
    internal class Program
    {
        private static string filename = "combinations.txt";

        static void Main(string[] args)
        {
            var deserializer = new DeserializerBuilder()
                                    .WithNamingConvention(new CamelCaseNamingConvention())
                                    //.WithTypeConverter(new TypeConverter())
                                    .IgnoreUnmatchedProperties()
                                    .Build();
            var patternParams = deserializer.Deserialize<PatternParams>(File.ReadAllText(args[0]));

            var passwordPattern = new PasswordPatternBuilder(2)
                .AddArbitraryPasswordSection("01", 3, 2, CharCase.UpperAndLower, new RussianToEnglishMapper())
                .Build();

            Console.WriteLine("Combinations: {0:N0}", passwordPattern.GetCombinationCount());
            Console.WriteLine($"Generate combinations into the file {filename}? (y/n)");
            if (Console.ReadLine()?.ToLower() == "n")
            {
                Console.WriteLine("Canceled");
                return;
            }
            
            using (var fileStream = new FileStream(filename, FileMode.Create))
            using (var writer = new StreamWriter(fileStream))
            {
                foreach (var combination in passwordPattern.GetCombinations())
                {
                    writer.WriteLine(combination);
                }
            }

            Console.WriteLine("Done");
        }
    }

    public class PatternParams
    {
        public string OutputFilename { get; set; }
        public int? MaxSingeCharSequence { get; set; }
        public SectionParamsBase[] Sections { get; set; }
    }


    public abstract class SectionParamsBase
    {
        public string Chars { get; set; }
        public string CharMapper { get; set; }
        public CharCase CharCase { get; set; }
    }

    public class ArbitrarySectionParams : SectionParamsBase
    {
        public int MaxLength { get; set; }
        public int MinLength { get; set; }
    }

    public class FixedSectionParams : SectionParamsBase
    {
    }

    //public class TypeConverter : IYamlTypeConverter
    //{
        
    //}
}
