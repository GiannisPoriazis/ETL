namespace ETL.Core.Models
{
    public class CustomerRecord
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public List<TransactionRecord> Transactions { get; set; } = new();

        public CustomerRecord(string name, string address, string zipCode)
        {
            Name = name;
            Address = address;
            ZipCode = zipCode;
        }
    }
}
