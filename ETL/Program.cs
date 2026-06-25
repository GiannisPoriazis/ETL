using Microsoft.Extensions.DependencyInjection;
using ETL.Core.Extensions;
using ETL.Core.Interfaces;

namespace ETL
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

            string inputFilePath = "source_data.dat";
            string outputFilePath = "processed_output.dat";

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
