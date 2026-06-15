using AcsExam.Core.Models;
using AcsExam.Core.Parsers;

namespace AcsExam.Tests.ParserTests
{
    public class TransactionLineParserTests
    {
        private readonly TransactionLineParser _parser;

        public TransactionLineParserTests()
        {
            _parser = new TransactionLineParser();
        }

        [Fact]
        public void Parse_ValidTransactionLine_ReturnsTransactionRecord()
        {
            // Arrange
            var line = "TRANSACTION|MOUSE|2";
            var parsedDocument = new ParsedDocument();
            parsedDocument.Customers.Add(new CustomerRecord("John Doe", "address", "12345"));

            // Act
            _parser.ParseInto(line, parsedDocument);

            // Assert
            Assert.Single(parsedDocument.Customers.First().Transactions);
            Assert.Equal("MOUSE", parsedDocument.Customers.First().Transactions.First().ItemName);
            Assert.Equal(2, parsedDocument.Customers.First().Transactions.First().Quantity);
        }

        [Fact]
        public void Parse_OrphanTransactionLine_ReturnsIndexOutOfRangeException()
        {
            // Arrange
            var line = "TRANSACTION|MOUSE|2";
            var parsedDocument = new ParsedDocument();

            // Act & Assert
            var exception = Assert.Throws<IndexOutOfRangeException>(() => _parser.ParseInto(line, new ParsedDocument()));
            Assert.Contains("Orphaned transaction line found.", exception.Message);
        }

        [Theory]
        [InlineData("TRANSACTION|MOUSE")]
        [InlineData("TRANSACTION MOUSE 2")]
        public void Parse_InvalidTransactionLine_ThrowsArgumentException(string line)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _parser.ParseInto(line, new ParsedDocument()));
        }

        [Fact]
        public void Parse_InvalidQuantityFormat_ThrowsFormatException()
        {
            // Arrange
            var line = "TRANSACTION|MOUSE|two";

            // Act & Assert
            var exception = Assert.Throws<FormatException>(() => _parser.ParseInto(line, new ParsedDocument()));
            Assert.Contains("Invalid quantity format", exception.Message);
        }
    }
}
