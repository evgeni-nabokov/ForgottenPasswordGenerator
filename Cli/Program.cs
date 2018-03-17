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
        private const string StatisticsFileExtension = "stat";
        private const ulong ChunkSize = 100_000;

        static void Main(string[] args)
        {
            var loader = new YamlParamLoader();
            var paramFilename = args[0];
            var filenameWithoutExtension = Path.GetFileNameWithoutExtension(paramFilename);
            var directoryName = Path.GetDirectoryName(paramFilename);
            var outputFilename = Path.Combine(
                directoryName,
                $"{filenameWithoutExtension}.{PasswordFileExtension}"
            );
            var statisticsFilename = Path.Combine(
                directoryName,
                $"{filenameWithoutExtension}.{StatisticsFileExtension}"
            );

            var patternParams = loader.Load(paramFilename);

            var passwordPattern = CreatePasswordPatternFromParams(patternParams);
            var totalCount = passwordPattern.Count;
            Console.WriteLine($"Total loops count: {totalCount:N0}");
            Console.WriteLine($"Generate variations and save them into the file {outputFilename}? (y/n)");
            if (Console.ReadLine()?.ToLower() != "y")
            {
                Console.WriteLine("Canceled");
                return;
            }

            var madeCount = 0ul;
            var counter = 0ul;
            using (var fileStream = new FileStream(outputFilename, FileMode.Create))
            using (var writer = new StreamWriter(fileStream))
            {
                do
                {
                    writer.WriteLine(passwordPattern.Current);
                    counter++;
                    if (counter >= ChunkSize)
                    {
                        madeCount += counter;
                        counter = 0;
                        PrintProgress(madeCount, totalCount);
                        Console.CursorLeft = 0;
                    }
                } while (passwordPattern.MoveNext());
            }

            if (counter > 0)
            {
                madeCount += counter;
                PrintProgress(madeCount, totalCount);
            }
            
            Console.WriteLine();

            using (var fileStream = new FileStream(statisticsFilename, FileMode.Create))
            using (var writer = new StreamWriter(fileStream))
            {
                writer.WriteLine(passwordPattern.VariationNumber);
            }
            
            Console.WriteLine($"{passwordPattern.VariationNumber:N0} variations saved into {outputFilename}.");
            Console.WriteLine($"Statistics saved into {statisticsFilename}.");
            Console.ReadKey();
        }

        private static void PrintProgress(ulong madeCount, ulong totalCount)
        {
            Console.Write("Progress: {0:N0} of {1:N0} ({2:N0}%)", madeCount, totalCount, (double)madeCount / totalCount * 100);
        }

        private static PasswordPattern CreatePasswordPatternFromParams(PatternParams patternParams)
        {
            var passwordPatternBuilder = new PasswordPatternBuilder(
                patternParams.MaxSingeCharSequenceLength,
                patternParams.MaxCapitalLetterSequenceLength,
                patternParams.MinCapitalCharDistance,
                CharMapperFactory.CreateCharMapper(patternParams.CharMapper)
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
                        p.CharCase
                    );
                }
                else if (sectionParams is StringListSectionParams)
                {
                    var p = sectionParams as StringListSectionParams;
                    passwordPatternBuilder.AddStringListPasswordSection(
                        p.StringList
                    );
                }
                else if (sectionParams is NumberRangeSectionParams)
                {
                    var p = sectionParams as NumberRangeSectionParams;
                    passwordPatternBuilder.AddNumberRangePasswordSection(
                        p.MinValue,
                        p.MaxValue,
                        p.Step
                    );
                }
                else if (sectionParams is ArbitrarySectionParams)
                {
                    var p = sectionParams as ArbitrarySectionParams;
                    passwordPatternBuilder.AddArbitraryPasswordSection(
                        p.Chars,
                        p.MaxLength,
                        p.MinLength,
                        p.CharCase
                    );
                }
            }

            return passwordPatternBuilder.Build();
        }
    }
}
