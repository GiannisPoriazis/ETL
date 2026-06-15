using AcsExam.Core.Interfaces;

namespace AcsExam.Core.Services
{
    public class FileSystem : IFileSystem
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public IEnumerable<string> ReadLines(string path)
        {
            return File.ReadLines(path);
        }

        public void WriteAllLines(string path, IEnumerable<string> lines)
        {
            File.WriteAllLines(path, lines);
        }
    }
}