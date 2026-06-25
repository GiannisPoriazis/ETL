using ETL.Core.Models;

namespace ETL.Core.Interfaces
{
    public interface ILineParser
    {
        /// <summary>
        /// Determines whether the specified raw file line can be handled by this parser.
        /// </summary>
        /// <param name="line">The raw text line extracted from the source file.</param>
        /// <returns><c>true</c> if the line format matches the record type supported by this parser; otherwise, <c>false</c>.</returns>
        bool CanParse(string line);
        /// <summary>
        /// Parses the raw text line and maps its delimited segments directly into the hierarchical state of the provided document.
        /// </summary>
        /// <param name="line">The raw text line to be deserialized.</param>
        /// <param name="parsedDocument">The master <see cref="ParsedDocument"/> instance accumulation context.</param>
        void ParseInto(string line, ParsedDocument parsedDocument);
        /// <summary>
        /// Transforms an entity from the document back into its equivalent pipe-delimited string format.
        /// </summary>
        /// <param name="document">The source <see cref="ParsedDocument"/> holding the operational state.</param> 
        /// <returns>A formatted, pipe-delimited string representing the specified model item.</returns>
        string Serialize(ParsedDocument document, int index = default, int group = default);
    }
}
