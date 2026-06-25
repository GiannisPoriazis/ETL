using ETL.Core.Models;
using ETL.Core.Parsers;

namespace ETL.Tests.ParserTests
{
    public class CustomerLineParserTests
    {
        private readonly CustomerLineParser _parser;

        public CustomerLineParserTests()
        {
            _parser = new CustomerLineParser();
        }

        [Theory]
        [InlineData("CUSTOMER|Giannis Poriazis|Athens|13562", "Giannis Poriazis", "Athens", "13562")]
        [InlineData("CUSTOMER|Gates Bill|ALEKSANDROY 32 KRYONERI|17332", "Gates Bill", "ALEKSANDROY 32 KRYONERI", "17332")]
        [InlineData("CUSTOMER|Elon Musk|Silicon Valley|54321", "Elon Musk", "Silicon Valley", "54321")]
        public void Parse_ValidCustomerLine_ReturnsCustomerObject(string line, string expectedName, string expectedAddress, string expectedZip)
        {
            //Arrange
            var parsedDocument = new ParsedDocument();

            // Act
            _parser.ParseInto(line, parsedDocument);

            // Assert
            Assert.Single(parsedDocument.Customers);
            Assert.Equal(expectedName, parsedDocument.Customers.First().Name);
            Assert.Equal(expectedAddress, parsedDocument.Customers.First().Address);
            Assert.Equal(expectedZip, parsedDocument.Customers.First().ZipCode);
        }

        [Theory]
        [InlineData("CUSTOMER Giannis Poriazis Athens 13562")]
        [InlineData("CUSTOMER|Gates Bill|ALEKSANDROY 32 KRYONERI")]
        public void Parse_InvalidCustomerLine_ThrowsArgumentException(string line)
        {            
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _parser.ParseInto(line, new ParsedDocument()));
        }
    }
}
