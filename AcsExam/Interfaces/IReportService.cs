using AcsExam.Core.Models;

namespace AcsExam.Core.Interfaces
{
    public interface IReportService
    {
        void PrintExecutionSummary(ParsedDocument document, string filePath, int totalLinesProcessed);
    }
}
