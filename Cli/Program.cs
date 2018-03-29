using System;
using System.IO;
using Cli.Params;
using Cli.Yaml;
using Lib.PasswordPattern;
using Lib.PasswordPattern.Suppression;
using Writer;

namespace Cli
{
    internal class Program
    {
        private const string PasswordFileExtension = "list";
        private const string StatisticsFileExtension = "stat";
        private const ulong ChunkSize = 500_000;

        static void Main(string[] args)
        {
            var loader = new YamlParamLoader();
            var paramFilename = args[0];
            var filenameWithoutExtension = Path.GetFileNameWithoutExtension(paramFilename);
            var directoryName = Path.GetDirectoryName(paramFilename);
            string outputFilename = string.Empty;

            IWriter writer;

            var patternParams = loader.Load(paramFilename);

            var passwordPattern = CreatePasswordPatternFromParams(patternParams);
            var totalCount = passwordPattern.Count;
            Console.WriteLine($"Loops total: {totalCount:N0}");

            if (patternParams.Output == OutputStream.File)
            {
                outputFilename = Path.Combine(
                    directoryName,
                    $"{filenameWithoutExtension}.{PasswordFileExtension}"
                );

                writer = new FileWriter(outputFilename);
                Console.WriteLine($"Write variations into the file {outputFilename}? (y/n)");
            }
            else
            {
                writer = new ConsoleWriter();
                Console.WriteLine("Write variations to console? (y/n)");
            }

            if (Console.ReadLine()?.ToLower() != "y")
            {
                Console.WriteLine("cancelled ");
                return;
            }

            var runningTotal = 0ul;
            ulong loopDiff;
            var lastLoopNumber = 0ul;
            
            do
            {
                writer.Write(passwordPattern.Current);
                loopDiff = passwordPattern.LoopNumber - lastLoopNumber;

                if (loopDiff >= ChunkSize)
                {
                    runningTotal += loopDiff;
                    lastLoopNumber = passwordPattern.LoopNumber;
                    PrintProgress(runningTotal, totalCount);
                    
                }
            } while (passwordPattern.MoveNext());

            writer.Dispose();

            loopDiff = passwordPattern.LoopNumber - lastLoopNumber;
            if (loopDiff > 0)
            {
                runningTotal += loopDiff;
                PrintProgress(runningTotal, totalCount);
            }

            Console.WriteLine();

            var statisticsFilePath = Path.Combine(
                directoryName,
                $"{filenameWithoutExtension}.{StatisticsFileExtension}");
            WriteStatisticsIntoFile(writer.Statistics, statisticsFilePath);

            if (patternParams.Output != OutputStream.File)
            {
                Console.WriteLine($"Statistics saved into {statisticsFilePath}.");
            }
            Console.WriteLine($"{writer.Statistics.Written:N0} of {writer.Statistics.Received:N0} variations saved into {writer.Destination}.");

            Console.WriteLine("Done.");
            Console.ReadKey();
        }

        private static void PrintProgress(ulong madeCount, ulong totalCount)
        {
            Console.CursorLeft = 0;
            Console.Write("Progress: {0:N0} of {1:N0} ({2:N0}%)", madeCount, totalCount, (double)madeCount / totalCount * 100);
        }

        private static void WriteStatisticsIntoFile(Statistics statistics, string filePath)
        {
            using (var statFileStream = new FileStream(filePath, FileMode.Create))
            using (var statWriter = new StreamWriter(statFileStream))
            {
                statWriter.WriteLine(statistics.Written);
                statWriter.WriteLine(statistics.Received);
            }
        }

        private static PasswordPattern CreatePasswordPatternFromParams(PatternParams patternParams)
        {
            var passwordPatternBuilder = new PasswordPatternBuilder()
                .SetCharMapper(CharMapperFactory.CreateCharMapper(patternParams.CharMapper));

            if (patternParams.Suppression != null)
            {
                var sup = patternParams.Suppression;
                passwordPatternBuilder.SetSuppressOptions(new SuppressOptions(
                    sup.ForbiddenDuplicateChars,
                    sup.AdjacentDuplicateMaxLength,
                    sup.CapitalAdjacentMaxLength,
                    sup.CapitalCharMinDistance,
                    sup.Regex
                ));
            }

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
                else if (sectionParams is CompoundSectionParams)
                {
                    var p = sectionParams as CompoundSectionParams;
                    passwordPatternBuilder.AddCompoundPasswordSection(
                        p.Chars,
                        p.CharCase
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
