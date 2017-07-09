using System;
using SystemWrapper.IO;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.
    */

    public class CSVReaderWriter : ICsvReaderWriter
    {
        private readonly ICsvReader _csvReader;
        private readonly ICsvWriter _csvWriter;

        public CSVReaderWriter()
        {
            _csvReader = new CsvReader(new FileWrap());
            _csvWriter = new CsvWriter();
        }
        public CSVReaderWriter(ICsvWriter writer, ICsvReader reader)
        {
            _csvWriter = writer;
            _csvReader = reader;
        }

        // Not moving enum to preserve backwards compatibility 
        [Flags]
        public enum Mode
        {
            Read = 1,
            Write = 2
        }

        public void Open(string fileName, Mode mode)
        {
            switch (mode)
            {
                case Mode.Read:
                    _csvReader.Open(fileName);
                    break;
                case Mode.Write:
                    _csvWriter.Open(fileName);
                    break;
                default:
                    throw new Exception("Unknown file mode for " + fileName);
            }
        }

        public void Write(params string[] columns)
        {
            _csvWriter.Write(columns);    
        }

        public bool Read(string column1, string column2)
        {
            return _csvReader.Read();
        }

        public bool Read(out string column1, out string column2)
        {
            if (_csvReader.Read())
            {
                column1 = _csvReader.LinesReadFromFile[0];
                column2 = _csvReader.LinesReadFromFile[1];
                return true;
            }
            column1 = null;
            column2 = null;
            return false;       
        }

        public void Close()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_csvReader != null)
            {
                _csvReader.Dispose();
            }
            if (_csvWriter != null)
            {
                _csvWriter.Dispose();
            }
        }
    }
}
