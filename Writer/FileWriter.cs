using System;
using System.IO;
using System.Text;

namespace Writer
{
    public class FileWriter : IWriter
    {
        private const int BufferSizeInBytes = 10_000_000;
        private readonly StreamWriter _streamWriter;

        public Statistics Statistics { get; } = new Statistics();
        public string Destination { get; }

        public FileWriter(string outputFilename, int bufferSizeInBytes = BufferSizeInBytes)
        {
            Destination = outputFilename;
            var outputStream = new FileStream(outputFilename, FileMode.Create);
            _streamWriter = new StreamWriter(outputStream, Encoding.UTF8, bufferSizeInBytes);
        }

        public void Write(string variation)
        {
            Statistics.Received++;
            _streamWriter.WriteLine(variation);
            Statistics.Written++;
        }

        public void Flush()
        {
            _streamWriter.Flush();
        }

        public void Dispose()
        {
            _streamWriter?.Dispose();
        }
    }
}
