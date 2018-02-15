using System;
using System.IO;
using Cli.Params;
using Cli.Yaml;
using Lib.PasswordPattern;

namespace Cli
{
    internal class Program
    {
        private static string defaultFilename = "passwords.txt";

        static void Main(string[] args)
        {
            var loader = new YamlParamLoader();
            var patternParams = loader.Load(args[0]);

            patternParams.OutputFilename = patternParams.OutputFilename?.Trim();
            if (string.IsNullOrEmpty(patternParams.OutputFilename))
            {
                patternParams.OutputFilename = defaultFilename;
            }

            var passwordPattern = CreatePasswordPatternFromParams(patternParams);

            Console.WriteLine("Combinations: {0:N0}", passwordPattern.GetCombinationCount());
            Console.WriteLine($"Generate combinations into the file {patternParams.OutputFilename}? (y/n)");
            if (Console.ReadLine()?.ToLower() == "n")
            {
                Console.WriteLine("Canceled");
                return;
            }
            
            using (var fileStream = new FileStream(patternParams.OutputFilename, FileMode.Create))
            using (var writer = new StreamWriter(fileStream))
            {
                foreach (var combination in passwordPattern.GetCombinations())
                {
                    writer.WriteLine(combination);
                }
            }

            Console.WriteLine("Done");
        }

        private static PasswordPattern CreatePasswordPatternFromParams(PatternParams patternParams)
        {
            var passwordPatternBuilder = new PasswordPatternBuilder(
                patternParams.Sections.Length,
                patternParams.MaxSingeCharSequence
            );

            for (var i = 0; i < patternParams.Sections.Length; i++)
            {
                var sectionParams = patternParams.Sections[i];
                if (sectionParams is FixedSectionParams)
                {
                    passwordPatternBuilder.AddFixedPasswordSection(sectionParams.Chars);
                }
                else if (sectionParams is ArbitrarySectionParams)
                {
                    var p = sectionParams as ArbitrarySectionParams;
                    passwordPatternBuilder.AddArbitraryPasswordSection(p.Chars, p.MaxLength, p.MinLength);
                }
            }

            return passwordPatternBuilder.Build();
        }
    }
}
