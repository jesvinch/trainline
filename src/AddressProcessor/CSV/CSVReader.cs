using System;
using SystemInterface.IO;

namespace AddressProcessing.CSV
{
    public class CsvReader : ICsvReader
    {
        private readonly IFile _fileWrap;
        private IStreamReader _streamReaderWrapper;
        
        public string[] LinesReadFromFile { get; private set; }

        public CsvReader(IFile fileWrap)
        {
            _fileWrap = fileWrap;
        }

        public void Open(string fileName)
        {
            _streamReaderWrapper = _fileWrap.OpenText(fileName);
        }

        public bool Read()
        {
            var line = _streamReaderWrapper.ReadLine();
            if (String.IsNullOrEmpty(line))
            {
                return false;
            }

            LinesReadFromFile = line.Split('\t');
            //Added extra check to guard against index out of range exception when line = oneWord. 
            if (LinesReadFromFile.Length == 0 || LinesReadFromFile.Length == 1)
            {
                return false;
            }
            return true;
        }

        public void Dispose()
        {
            if (_streamReaderWrapper != null)
            {
                _streamReaderWrapper.Close();
            }
        }
    }
}
