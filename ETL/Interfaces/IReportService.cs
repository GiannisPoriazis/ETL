using ETL.Core.Models;

namespace ETL.Core.Interfaces
{
    public interface IReportService
    {
        void PrintExecutionSummary(ParsedDocument document, string filePath, int totalLinesProcessed);
    }
}
