using System;

namespace AddressProcessing.CSV
{
    public interface ICsvReader : IDisposable
    {
        void Open(string fileName);
        bool Read();
        string[] LinesReadFromFile { get; }
    }
}
