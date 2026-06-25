using ETL.Core.Interfaces;
using ETL.Core.Models;

namespace ETL.Core.Parsers
{
    public class CustomerLineParser: ILineParser
    {
        private const string RECORD_TYPE = "CUSTOMER";

        public bool CanParse(string line) => line.StartsWith(RECORD_TYPE);

        public void ParseInto(string line, ParsedDocument parsedDocument)
        {
            var parts = line.Split('|');

            if (parts.Length != 4)
            {
                throw new ArgumentException("Invalid customer line format.");
            }

            parsedDocument.Customers.Add(new CustomerRecord(parts[1], parts[2], parts[3]));
        }

        public string Serialize(ParsedDocument document, int index, int group = default)
        {
            if (index < 0 || index >= document.Customers.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Target customer index does not exist.");
            }

            var customer = document.Customers[index];

            return $"{RECORD_TYPE}|{customer.Name}|{customer.Address}|{customer.ZipCode}";
        }
    }
}
