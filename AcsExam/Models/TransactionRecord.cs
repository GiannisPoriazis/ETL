namespace AcsExam.Core.Models
{
    public class TransactionRecord
    {
        public string ItemName { get; set; }
        public short Quantity { get; set; }
        public TransactionRecord(string itemName, short quantity)
        {
            ItemName = itemName;
            Quantity = quantity;
        }
    }
}
