using AcsExam.Core.Interfaces;
using AcsExam.Core.Models;
using AcsExam.Core.Parsers;
using AcsExam.Core.Services;
using Moq;

namespace AcsExam.Tests.ProcessorTests
{
    public class FileProcessorTests
    {
        private readonly FileProcessor _fileProcessor;
        private readonly Mock<IFileSystem> _mockFileSystem;
        private readonly Mock<IReportService> _mockReportService;
        private readonly List<ILineParser> _parsers;

        public FileProcessorTests()
        {
            _mockFileSystem = new Mock<IFileSystem>();
            _mockReportService = new Mock<IReportService>();

            _parsers = new List<ILineParser>
            {
                new HeaderLineParser(),
                new CustomerLineParser(),
                new TransactionLineParser(),
                new FooterLineParser()
            };

            _fileProcessor = new FileProcessor(
                _parsers,
                _mockFileSystem.Object,
                _mockReportService.Object);
        }

        [Fact]
        public void Deserialize_ValidFile_ReturnsExpectedResults()
        {
            // Arrange
            string filePath = "test.dat";

            var mockLines = new[] {
                "BEGINFILE|20260615132745123", 
                "CUSTOMER|John Doe|Address 1|12345",
                "ENDFILE|CUSTOMER_COUNT=5|TRANSACTION_COUNT=0"
            };

            _mockFileSystem.Setup(f => f.FileExists(filePath)).Returns(true);
            _mockFileSystem.Setup(f => f.ReadLines(It.IsAny<string>())).Returns(mockLines);

            // Act
            var document = _fileProcessor.Deserialize(filePath);

            // Assert
            Assert.Single(document.Customers);
            Assert.Equal("John Doe", document.Customers.First().Name);
            Assert.Equal("Address 1", document.Customers.First().Address);
            Assert.Equal("12345", document.Customers.First().ZipCode);
            Assert.Equal(5, document.ExpectedCustomerCount);
            Assert.Equal(0, document.ExpectedTransactionCount);
        }

        [Fact]
        public void Deserialize_InvalidFile_ThrowsException()
        {
            // Arrange
            string filePath = "test.dat";

            var mockLines = new[] {
                "BEGINFILE|20260615132745123",
                "CUSTOMER|John Doe|Address 1|12345",
                "ENDFILE|CUSTOMER_COUNT=5|TRANSACTION_COUNT=ZERO"
            };

            _mockFileSystem.Setup(f => f.FileExists(filePath)).Returns(true);
            _mockFileSystem.Setup(f => f.ReadLines(It.IsAny<string>())).Returns(mockLines);

            // Act & Assert
            Assert.Throws<Exception>(() => _fileProcessor.Deserialize(filePath));
        }
    }
}
