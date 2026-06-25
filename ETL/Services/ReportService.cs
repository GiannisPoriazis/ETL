using ETL.Core.Interfaces;
using ETL.Core.Models;
using System.Text;

namespace ETL.Core.Services
{
    public class ReportService : IReportService
    {
        public void PrintExecutionSummary(ParsedDocument document, string filePath, int totalLinesProcessed)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine($"\n{new string('=', 50)}");
            Console.WriteLine("\t\tEXECUTION REPORT\t\t");
            Console.WriteLine(new string('=', 50));
            Console.ResetColor();

            Console.WriteLine($"\nFILE PATH: {document.Metadata.Path}");
            Console.WriteLine($"FILE NAME: {document.Metadata.Name}");

            if (document.Metadata.Timestamp.HasValue)
            {
                Console.WriteLine($"CREATION DATE: {document.Metadata.Timestamp.Value:dddd, dd MMMM yyyy HH:mm:ss}");
            }

            Console.WriteLine(new string('-', 50));

            Console.WriteLine("TOTAL QUANTITY PER ITEM:");
            var itemTotals = document.Customers
                .SelectMany(c => c.Transactions)
                .GroupBy(t => t.ItemName)
                .Select(g => new { Item = g.Key, Quantity = g.Sum(t => t.Quantity) });

            foreach (var summary in itemTotals)
            {
                Console.WriteLine($"  * Item: {summary.Item,-25} | Total Quantity: {summary.Quantity}");
            }

            Console.WriteLine(new string('-', 50));

            bool customersMatch = document.Customers.Count == document.ExpectedCustomerCount;
            bool transactionsMatch = document.TotalTransactionsCount == document.ExpectedTransactionCount;

            PrintChecksumLine("CUSTOMER COUNT", document.Customers.Count, document.ExpectedCustomerCount, customersMatch);
            PrintChecksumLine("TRANSACTION COUNT", document.TotalTransactionsCount, document.ExpectedTransactionCount, transactionsMatch);

            if (customersMatch && transactionsMatch)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n[STATUS]: SUCCESS - All file line metrics match the footer checksum declarations.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[STATUS]: CRITICAL - Checksum mismatch detected! File may be altered or incomplete.");
            }
            Console.ResetColor();
            Console.WriteLine(new string('=', 50) + "\n");
        }

        private void PrintChecksumLine(string label, int actual, int expected, bool isMatch)
        {
            Console.Write($"  * {label,-18} -> Parsed: {actual,-4} | Expected in Footer: {expected,-4} [");
            if (isMatch)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("MATCH");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("MISMATCH");
            }
            Console.ResetColor();
            Console.WriteLine("]");
        }
    }
}