namespace AcsExam.Core.Models
{
    public class FileMetadata
    {
        public string Path { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime? Timestamp { get; set; }
    }
}
