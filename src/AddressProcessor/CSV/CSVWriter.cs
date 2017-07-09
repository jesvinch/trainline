using System.IO;

namespace AddressProcessing.CSV
{
    public class CsvWriter : ICsvWriter
    {
        private StreamWriter _streamWriter;
        public void Open(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            _streamWriter = fileInfo.CreateText();
        }

        public void Write(params string[] columns)
        {
            var lineToWrite = string.Join("\t", columns);
            _streamWriter.WriteLine(lineToWrite);
        }

        public void Dispose()
        {
            if (_streamWriter != null)
            {
                _streamWriter.Close();
            }
        }
    }
}
