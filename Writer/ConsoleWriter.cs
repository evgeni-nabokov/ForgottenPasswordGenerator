using System;
using System.Collections.Generic;
using System.Text;

namespace Writer
{
    public class ConsoleWriter : IWriter
    {
        public Statistics Statistics { get; } = new Statistics();
        public string Destination { get; }

        public ConsoleWriter()
        {
            Destination = "Console";
        }

        public void Write(string variation)
        {
            Statistics.Received++;
            Console.WriteLine(variation);
            Statistics.Written++;
        }

        public void Flush()
        {
        }

        public void Dispose()
        {
        }
    }
}
