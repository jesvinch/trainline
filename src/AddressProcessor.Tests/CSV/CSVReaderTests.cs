using SystemWrapper.IO;
using AddressProcessing.CSV;
using Moq;
using NUnit.Framework;

namespace AddressProcessing.Tests.CSV
{
    [TestFixture]
    public class CsvReaderTests
    {
        private const string TestInputFile = @"test_data\contacts.csv";
        private const string TestFirstLineEmptyFile = @"test_data\firstLineEmptyFile.csv";
        private const string TestOneWordFile = @"test_data\oneWordFile.csv";

        [Test]
        public void Test_Read_ReadsFirstLineFromFile_ForValidFile()
        {
            var fileWrap = new Mock<IFileWrap>();
            var streamWriterWrap = new Mock<IStreamReaderWrap>();
            fileWrap.Setup(x => x.OpenText(TestInputFile)).Returns(streamWriterWrap.Object);
            streamWriterWrap.Setup(x => x.ReadLine()).Returns("Shelby Macias	3027 Lorem St.|Kokomo");

            using (var reader = new CsvReader(fileWrap.Object))
            {
                reader.Open(TestInputFile);
                reader.Read();

                Assert.AreEqual("Shelby Macias", reader.LinesReadFromFile[0]);
                Assert.AreEqual("3027 Lorem St.|Kokomo", reader.LinesReadFromFile[1]);
            }
        }

        [Test]
        public void Test_Dispose_StreamReaderDisposeIsCalled()
        {

            var fileWrap = new Mock<IFileWrap>();
            var streamWriterWrap = new Mock<IStreamReaderWrap>();
            fileWrap.Setup(x => x.OpenText(TestInputFile)).Returns(streamWriterWrap.Object);
            streamWriterWrap.Setup(x => x.ReadLine()).Returns("Shelby Macias	3027 Lorem St.|Kokomo");

            var reader = new CsvReader(fileWrap.Object);
            reader.Open(TestInputFile);
            reader.Read();
            reader.Dispose();

            streamWriterWrap.Verify(x => x.Close());
        }

        [Test]
        public void IntegrationTest_Read_ReadsFirstLineFromFile_ForValidFile()
        {
            using (var reader = new CsvReader(new FileWrap()))
            {
                reader.Open(TestInputFile);
                reader.Read();

                Assert.AreEqual("Shelby Macias", reader.LinesReadFromFile[0]);
                Assert.AreEqual("3027 Lorem St.|Kokomo|Hertfordshire|L9T 3D5|England", reader.LinesReadFromFile[1]);
            }
        }

        [Test]
        public void IntegrationTest_Read_ReturnsFalse_ForFirstLineEmptyFile()
        {
            using (var reader = new CsvReader(new FileWrap()))
            {
                reader.Open(TestFirstLineEmptyFile);
                var result = reader.Read();

                Assert.AreEqual(false, result);
            }
        }

        [Test]
        public void IntegrationTest_Read_ReturnsFalse_ForFileContainingOneWord()
        {
            using (var reader = new CsvReader(new FileWrap()))
            {
                reader.Open(TestOneWordFile);
                var result = reader.Read();

                Assert.AreEqual(false, result);
            }
        }
    }
}
