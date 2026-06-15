using AcsExam.Core.Models;
using AcsExam.Core.Parsers;

namespace AcsExam.Tests.ParserTests
{
    public class FooterLineParserTests
    {
        private readonly FooterLineParser _parser;

        public FooterLineParserTests()
        {
            _parser = new FooterLineParser();
        }

        [Theory]
        [InlineData("ENDFILE|CUSTOMER_COUNT=3|TRANSACTION_COUNT=9", 3, 9)]
        [InlineData("ENDFILE|CUSTOMER_COUNT=8|TRANSACTION_COUNT=1", 8, 1)]
        public void Parse_ValidFooterLine_Parses_Checksum(string line, short expectedCustomerCount, short expectedTransactionsCount)
        {
            //Arrange
            var parsedDocument = new ParsedDocument();

            // Act
            _parser.ParseInto(line, parsedDocument);

            // Assert
            Assert.Equal(expectedCustomerCount, parsedDocument.ExpectedCustomerCount);
            Assert.Equal(expectedTransactionsCount, parsedDocument.ExpectedTransactionCount);
        }

        [Theory]
        [InlineData("ENDFILE CUSTOMER_COUNT=3 TRANSACTION_COUNT=9")]
        [InlineData("ENDFILE|CUSTOMER_COUNT=8 TRANSACTION_COUNT=1")]
        public void Parse_InvalidFooterLine_ThrowsArgumentException(string line)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _parser.ParseInto(line, new ParsedDocument()));
        }

        [Theory]
        [InlineData("ENDFILE|CUSTOMER_COUNT|TRANSACTION_COUNT=9")]
        [InlineData("ENDFILE|3|TRANSACTION_COUNT=9")]
        [InlineData("ENDFILE|CUSTOMER_COUNT=3|TRANSACTION_COUNT9")]
        [InlineData("ENDFILE|CUSTOMER_COUNT=3|9")]
        [InlineData("ENDFILE|CUSTOMER_COUNT=3|TRANSACTION_COUNT=NINE")]
        public void Parse_InvalidChecksumFormat_ThrowsFormatException(string line)
        {
            // Act & Assert
            Assert.Throws<FormatException>(() => _parser.ParseInto(line, new ParsedDocument()));
        }
    }
}
