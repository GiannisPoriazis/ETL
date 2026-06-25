using ETL.Core.Models;
using ETL.Core.Parsers;

namespace ETL.Tests.ParserTests
{
    public class HeaderLineParserTests
    {
        private readonly HeaderLineParser _parser;

        public HeaderLineParserTests()
        {
            _parser = new HeaderLineParser();
        }

        [Theory]
        [InlineData("BEGINFILE|20210224143745345", 2021, 02, 24, 14, 37, 45, 345)]
        [InlineData("BEGINFILE|20211231235959999", 2021, 12, 31, 23, 59, 59, 999)]
        public void Parse_ValidHeaderLine_ReturnsCorrectDateTime(string headerLine, int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            // Arrange
            var parsedDocument = new ParsedDocument();

            // Act
            _parser.ParseInto(headerLine, parsedDocument);

            // Assert
            Assert.Equal(new DateTime(year, month, day, hour, minute, second, millisecond), parsedDocument.Metadata.Timestamp);
        }

        [Fact]
        public void Parse_InvalidHeaderLine_ThrowsIndexOutOfRangeException()
        {
            // Arrange
            var headerLine = "BEGINFILE 20210224143745345"; 

            // Act & Assert
            Assert.Throws<IndexOutOfRangeException>(() => _parser.ParseInto(headerLine, new ParsedDocument()));
        }

        [Fact]
        public void Parse_InvalidTimestamp_ThrowsFormatException()
        {
            // Arrange
            var headerLine = "|BEGINFILE 20210224143745345"; 

            // Act & Assert
            Assert.Throws<FormatException>(() => _parser.ParseInto(headerLine, new ParsedDocument()));
        }
    }
}
