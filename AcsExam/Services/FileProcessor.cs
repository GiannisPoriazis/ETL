using AcsExam.Core.Interfaces;
using AcsExam.Core.Models;
using AcsExam.Core.Parsers;

namespace AcsExam.Core.Services
{
    public class FileProcessor: IFileProcessor
    {
        private readonly IEnumerable<ILineParser> _parsers;
        private readonly IFileSystem _fileSystem;
        private readonly IReportService _reportService;

        public FileProcessor(
            IEnumerable<ILineParser> parsers,
            IFileSystem fileSystem,
            IReportService reportService)
        {
            _parsers = parsers;
            _fileSystem = fileSystem;
            _reportService = reportService;
        }

        public ParsedDocument Deserialize(string path)
        {
            if (!_fileSystem.FileExists(path))
            {
                throw new FileNotFoundException($"Target file not found: {path}");
            }

            var document = new ParsedDocument();
            var fileInfo = new FileInfo(path);

            document.Metadata.Path = fileInfo.FullName;
            document.Metadata.Name = fileInfo.Name;

            int lineCount = 0;

            foreach (var line in _fileSystem.ReadLines(path))
            {
                Console.WriteLine(line);
                lineCount++;

                if (line != null)
                {
                    ILineParser? parser = _parsers.FirstOrDefault(p => p.CanParse(line));

                    if (parser != null)
                    {
                        try
                        {
                            parser?.ParseInto(line, document);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    }
                }
            }

            _reportService.PrintExecutionSummary(document, path, lineCount);

            return document;
        }

        public void Serialize(ParsedDocument document, string outputPath)
        {
            var outputLines = new List<string>();

            var headerParser = _parsers.First(p => p.CanParse("BEGINFILE"));
            outputLines.Add(headerParser.Serialize(document, 0, 0));

            var customerParser = _parsers.First(p => p.CanParse("CUSTOMER"));
            for (int i = 0; i < document.Customers.Count; i++)
            {
                outputLines.Add(customerParser.Serialize(document, i, 0));

                var transactionParser = _parsers.First(p => p.CanParse("TRANSACTION"));
                for (int y = 0; y < document.Customers.ElementAt(i).Transactions.Count; y++)
                {
                    outputLines.Add(transactionParser.Serialize(document, y, i));
                }
            }

            var footerParser = _parsers.First(p => p.CanParse("ENDFILE"));
            outputLines.Add(footerParser.Serialize(document, 0, 0));

            _fileSystem.WriteAllLines(outputPath, outputLines);
        }
    }
}
