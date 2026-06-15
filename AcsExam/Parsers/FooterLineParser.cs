using AcsExam.Core.Interfaces;
using AcsExam.Core.Models;

namespace AcsExam.Core.Parsers
{
    public class FooterLineParser: ILineParser
    {
        private const string RECORD_TYPE = "ENDFILE";

        public bool CanParse(string line) => line.StartsWith(RECORD_TYPE);

        public void ParseInto(string line, ParsedDocument parsedDocument)
        {
            var parts = line.Split('|');

            if (parts.Length != 3)
            {
                throw new ArgumentException("Invalid footer line format");
            }

            short expectedCustomerCount;
            short expectedTransactionCount;

            try
            {
                var argumentParts = parts[1].Split('=');

                if (argumentParts.Length != 2 || !short.TryParse(argumentParts[1], out expectedCustomerCount))
                {
                    throw new FormatException("Invalid customer count format");
                }
            }
            catch (FormatException ex)
            {
                throw new FormatException($"Invalid customer count format: {parts[1]}", ex);
            }

            try
            {
                var argumentParts = parts[2].Split('=');

                if (argumentParts.Length != 2 || !short.TryParse(argumentParts[1], out expectedTransactionCount))
                {
                    throw new FormatException("Invalid transaction count format");
                }
            }
            catch (FormatException ex)
            {
                throw new FormatException($"Invalid transaction count format: {parts[2]}", ex);
            }

            parsedDocument.ExpectedCustomerCount = expectedCustomerCount;
            parsedDocument.ExpectedTransactionCount = expectedTransactionCount;
        }

        public string Serialize(ParsedDocument document, int _, int group)
        {
            int customerCount = document.Customers.Count;
            int transactionCount = document.TotalTransactionsCount;

            return $"{RECORD_TYPE}|CUSTOMER_COUNT={customerCount}|TRANSACTION_COUNT={transactionCount}";
        }
    }
}