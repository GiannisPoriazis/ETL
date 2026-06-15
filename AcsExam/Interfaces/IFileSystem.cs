namespace AcsExam.Core.Interfaces
{
    public interface IFileSystem
    {
        IEnumerable<string> ReadLines(string path);
        void WriteAllLines(string path, IEnumerable<string> lines);
        bool FileExists(string path);
    }
}