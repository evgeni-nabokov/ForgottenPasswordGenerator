using System;
using System.IO;
using Cli.Params;
using Cli.Yaml;
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

            IWriter writer;

            var programParams = loader.Load(paramFilename);

            var generator = VariationGeneratorFactory.CreateVariationGeneratorFromParams(programParams);

            var totalCount = generator.LoopCount;
            Console.WriteLine($"Loop count: {totalCount:N0}");

            if (programParams.Output == OutputStream.File)
            {
                var outputFilename = Path.Combine(
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
                writer.Write(generator.Current);
                loopDiff = generator.LoopNumber - lastLoopNumber;

                if (loopDiff >= ChunkSize)
                {
                    runningTotal += loopDiff;
                    lastLoopNumber = generator.LoopNumber;
                    PrintProgress(runningTotal, totalCount);

                }
            } while (generator.MoveNext());

            writer.Dispose();

            loopDiff = generator.LoopNumber - lastLoopNumber;
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

            if (programParams.Output != OutputStream.File)
            {
                Console.WriteLine($"Statistics saved into {statisticsFilePath}.");
            }
            Console.WriteLine(
                $"{writer.Statistics.Written:N0} of {writer.Statistics.Received:N0} variations saved into {writer.Destination}.");

            Console.WriteLine("Done.");
            Console.ReadKey();
        }

        private static void PrintProgress(ulong madeCount, ulong totalCount)
        {
            Console.CursorLeft = 0;
            Console.Write("Progress: {0:N0} of {1:N0} ({2:N0}%)", madeCount, totalCount,
                (double) madeCount / totalCount * 100);
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
    }
}
