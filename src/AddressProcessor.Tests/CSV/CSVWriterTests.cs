using SystemWrapper.IO;
using AddressProcessing.CSV;
using NUnit.Framework;

namespace AddressProcessing.Tests.CSV
{
    [TestFixture]
    public class CsvWriterTests
    {
        private const string WriteToFile = @"test_data\writeWordTest.csv";

        /// <summary>
        /// Test writing to file. 
        /// </summary>
        [Test]
        public void Test_Write_WritesToFile()
        {
            var writer = new CsvWriter();
            writer.Open(WriteToFile);
            writer.Write("FirstWord", "SecondWord");
            writer.Dispose();

            using (var reader = new CsvReader(new FileWrap()))
            {
                reader.Open(WriteToFile);
                reader.Read();

                Assert.AreEqual("FirstWord", reader.LinesReadFromFile[0]);
                Assert.AreEqual("SecondWord", reader.LinesReadFromFile[1]);
            }
        }
    }
}
