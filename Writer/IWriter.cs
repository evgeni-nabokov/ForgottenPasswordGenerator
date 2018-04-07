using System;

namespace Writer
{
    public interface IWriter : IDisposable
    {
        void Write(string variation);
        void Flush();
        void Close();
        string Destination { get; }
        Statistics Statistics { get; }
    }
}
