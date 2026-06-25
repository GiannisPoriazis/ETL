namespace ETL.Core.Models
{
    public class ParsedDocument
    {
        public FileMetadata Metadata { get; set; } = new();
        public List<CustomerRecord> Customers { get; set; } = new();
        public int TotalTransactionsCount => Customers.Sum(c => c.Transactions.Count);
        public short ExpectedCustomerCount { get; set; }
        public short ExpectedTransactionCount { get; set; }
    }
}
