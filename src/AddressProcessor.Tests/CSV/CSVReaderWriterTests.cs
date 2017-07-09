using AddressProcessing.CSV;
using Moq;
using NUnit.Framework;

namespace Csv.Tests
{
    [TestFixture]
    public class CsvReaderWriterTests
    {
        private const string TestInputFile = @"test_data\contacts.csv";
        private Mock<ICsvReader> _csvReader;
        private Mock<ICsvWriter> _csvWriter;

        [TestFixtureSetUp]
        public void TestInitialize()
        {
            _csvReader = new Mock<ICsvReader>();
            _csvWriter = new Mock<ICsvWriter>();
        }

        [Test]
        public void Test_Write_CallsCSVWritersWriteMethod()
        {
            var writerReader = new CSVReaderWriter(_csvWriter.Object, _csvReader.Object);
            writerReader.Write();

            _csvWriter.Verify(x => x.Write(), Times.AtLeastOnce);
            writerReader.Close();
        }

        [Test]
        public void Test_Read_CallsCSVReadersReadMethod()
        {
            _csvReader.Setup(x => x.Read()).Returns(false);
            var writerReader = new CSVReaderWriter(_csvWriter.Object, _csvReader.Object);
            var result = writerReader.Read("test", "test");
            writerReader.Close();

            _csvReader.Verify(x => x.Read(), Times.AtLeastOnce);
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Test_Close_CallsDisposeOnReaderAndWriter()
        {
            var writerReader = new CSVReaderWriter(_csvWriter.Object, _csvReader.Object);
            writerReader.Close();

            _csvWriter.Verify(x => x.Dispose(), Times.AtLeastOnce);
            _csvReader.Verify(x => x.Dispose(), Times.AtLeastOnce);
        }

        [Test]
        public void Test_Dispose_CallsDisposeOnReaderAndWriter()
        {
            var writerReader = new CSVReaderWriter(_csvWriter.Object, _csvReader.Object);
            writerReader.Dispose();

            _csvWriter.Verify(x => x.Dispose(), Times.AtLeastOnce);
            _csvReader.Verify(x => x.Dispose(), Times.AtLeastOnce);
        }

        /// <summary>
        /// Integration Test. End to end integration test for CSVReader
        /// </summary>
        [Test]
        public void Test_Read_ReadsDataFromFile_ForValidFile()
        {
            var writerReader = new CSVReaderWriter();
            writerReader.Open(TestInputFile, CSVReaderWriter.Mode.Read);
            string column1, column2;
            writerReader.Read(out column1, out column2);

            Assert.AreEqual("Shelby Macias", column1);
            Assert.AreEqual("3027 Lorem St.|Kokomo|Hertfordshire|L9T 3D5|England", column2);
        }
    }
}
