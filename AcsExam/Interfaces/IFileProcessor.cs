using AcsExam.Core.Models;

namespace AcsExam.Core.Interfaces
{
    public interface IFileProcessor
    {
        /// <summary>
        /// Reads a file sequentially, executes validation metrics, 
        /// prints rows to the console, and compiles a comprehensive hierarchical data object.
        /// </summary>
        /// <param name="inputPath">The relative or absolute file path to the source dataset.</param>
        /// <returns>A structured <see cref="ParsedDocument"/> representing the file data.</returns>
        /// <exception cref="FileNotFoundException">Thrown when the target file does not exist at the designated path.</exception>
        /// <exception cref="FormatException">Thrown if internal line metrics or cross-reference file checksum validations fail.</exception>
        ParsedDocument Deserialize(string inputPath);
        /// <summary>
        /// Serializes a structured data object into a pipe-delimited text sequence 
        /// while dynamically generating all record summaries and required footer compliance checksums.
        /// </summary>
        /// <param name="document">The populated <see cref="ParsedDocument"/> model container.</param>
        /// <param name="outputPath">The destination target file path where the generated dataset will be created or overwritten.</param>
        void Serialize(ParsedDocument document, string outputPath);
    }
}
