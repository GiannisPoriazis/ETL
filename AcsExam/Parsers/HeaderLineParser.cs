using AcsExam.Core.Interfaces;
using AcsExam.Core.Models;
using Microsoft.VisualBasic;
using System.Globalization;

namespace AcsExam.Core.Parsers
{
    public class HeaderLineParser: ILineParser
    {
        private const string RECORD_TYPE = "BEGINFILE";
        private const string DATE_FORMAT = "yyyyMMddHHmmssfff";
        public bool CanParse(string line) => line.StartsWith(RECORD_TYPE);

        public void ParseInto(string line, ParsedDocument parsedDocument)
        {
            var timestamp = string.Empty;
            try
            {
                timestamp = line.Split('|')[1];
                parsedDocument.Metadata.Timestamp = DateTime.ParseExact(timestamp, DATE_FORMAT, CultureInfo.InvariantCulture);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new IndexOutOfRangeException($"Invalid header line format: {line}", ex);
            }
            catch (FormatException ex)
            {
                throw new FormatException($"Invalid timestamp format: {timestamp}", ex);
            }
        }

        public string Serialize(ParsedDocument document, int index = default, int group = default)
        {
            if (document.Metadata == null || !document.Metadata.Timestamp.HasValue)
            {
                throw new InvalidOperationException("File metadata is missing; cannot serialize header.");
            }

            string formattedDate = document.Metadata.Timestamp.Value.ToString(DATE_FORMAT, CultureInfo.InvariantCulture);

            return $"{RECORD_TYPE}|{formattedDate}";
        }
    }
}
