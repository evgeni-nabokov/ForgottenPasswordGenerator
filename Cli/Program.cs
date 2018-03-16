using System;
using System.IO;
using Cli.Params;
using Cli.Yaml;
using Lib.PasswordPattern;

namespace Cli
{
    internal class Program
    {
        private const string PasswordFileExtension = "pwd";

        static void Main(string[] args)
        {
            var loader = new YamlParamLoader();
            var paramFilename = args[0];
            var outputFilename = Path.Combine(
                Path.GetDirectoryName(paramFilename),
                $"{Path.GetFileNameWithoutExtension(paramFilename)}.{PasswordFileExtension}"
            );
            var patternParams = loader.Load(paramFilename);

            var passwordPattern = CreatePasswordPatternFromParams(patternParams);

            Console.WriteLine("Max variations number: {0:N0}", passwordPattern.Count);
            Console.WriteLine($"Generate variations and save them into the file {outputFilename}? (y/n)");
            if (Console.ReadLine()?.ToLower() == "n")
            {
                Console.WriteLine("Canceled");
                return;
            }
            
            using (var fileStream = new FileStream(outputFilename, FileMode.Create))
            using (var writer = new StreamWriter(fileStream))
            {
                foreach (var combination in passwordPattern.GetVariations())
                {
                    writer.WriteLine(combination);
                }
            }

            Console.WriteLine("Done");
        }

        private static PasswordPattern CreatePasswordPatternFromParams(PatternParams patternParams)
        {
            var passwordPatternBuilder = new PasswordPatternBuilder(
                patternParams.MaxSingeCharSequenceLength,
                patternParams.MaxCapitalLetterSequenceLength,
                patternParams.MinCapitalCharDistance
            );

            for (var i = 0; i < patternParams.Sections.Length; i++)
            {
                var sectionParams = patternParams.Sections[i];
                if (sectionParams is FixedSectionParams)
                {
                    var p = sectionParams as FixedSectionParams;
                    passwordPatternBuilder.AddFixedPasswordSection(
                        p.Chars,
                        p.MinLength,
                        p.CharCase,
                        CharMapperFactory.CreateCharMapper(p.CharMapper));
                }
                else if (sectionParams is ArbitrarySectionParams)
                {
                    var p = sectionParams as ArbitrarySectionParams;
                    passwordPatternBuilder.AddArbitraryPasswordSection(
                        p.Chars,
                        p.MaxLength,
                        p.MinLength,
                        p.CharCase,
                        CharMapperFactory.CreateCharMapper(p.CharMapper));
                }
            }

            return passwordPatternBuilder.Build();
        }
    }
}
