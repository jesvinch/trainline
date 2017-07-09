using System;

namespace AddressProcessing.CSV
{
    public interface ICsvReaderWriter : IDisposable
    {
        void Open(string fileName, CSVReaderWriter.Mode mode);
        void Write(params string[] columns);
        bool Read(string column1, string column2);
        bool Read(out string column1, out string column2);
        void Close();
    }
}
