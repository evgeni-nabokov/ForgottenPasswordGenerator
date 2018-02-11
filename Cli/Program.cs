using System;
using System.IO;
using Lib;
using Lib.CharMappers;
using Lib.PasswordSections;

namespace Cli
{
    internal class Program
    {
        private static string filename = "combinations.key";

        static void Main(string[] args)
        {
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

    
}
