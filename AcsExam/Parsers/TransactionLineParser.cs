using AcsExam.Core.Interfaces;
using AcsExam.Core.Models;
using System.Reflection.Metadata;

namespace AcsExam.Core.Parsers
{
    public class TransactionLineParser: ILineParser
    {
        private const string RECORD_TYPE = "TRANSACTION";

        public bool CanParse(string line) => line.StartsWith(RECORD_TYPE);

        public void ParseInto(string line, ParsedDocument parsedDocument)
        {
            var parts = line.Split('|');

            if (parts.Length != 3)
            {
                throw new ArgumentException("Invalid transaction line format");
            }

            string itemName = parts[1];
            short quantity;

            try
            {
                quantity = short.Parse(parts[2]);
            }
            catch (FormatException ex)
            {
                throw new FormatException($"Invalid quantity format: {parts[2]}", ex);
            }

            if (parsedDocument.Customers.Count > 0)
            {
                parsedDocument.Customers.Last().Transactions.Add(new TransactionRecord(itemName, quantity));
            }
            else
            {
                throw new IndexOutOfRangeException("Orphaned transaction line found.");
            }
        }

        public string Serialize(ParsedDocument document, int index, int customer)
        {
            if (index < 0 || index >= document.Customers.ElementAt(customer).Transactions.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Target transaction index does not exist.");
            }

            var transaction = document.Customers.ElementAt(customer).Transactions[index];

            return $"{RECORD_TYPE}|{transaction.ItemName}|{transaction.Quantity}";
        }
    }
}
