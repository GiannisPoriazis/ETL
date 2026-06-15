using Microsoft.Extensions.DependencyInjection;
using AcsExam.Core.Extensions;
using AcsExam.Core.Interfaces;

namespace AcsExam
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddApplicationServices()
                .BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();
            var processor = scope.ServiceProvider.GetRequiredService<IFileProcessor>();

            string inputFilePath = "ACS_EXAM_READ_FILE.dat";
            string outputFilePath = "ACS_EXAM_EXPORT_FILE.dat";

            try
            {
                var parsedData = processor.Deserialize(inputFilePath);
                processor.Serialize(parsedData, outputFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Execution critical failure: {ex.Message}");
            }
        }
    }
}
