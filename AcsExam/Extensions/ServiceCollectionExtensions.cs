using Microsoft.Extensions.DependencyInjection;
using AcsExam.Core.Interfaces;
using AcsExam.Core.Parsers;
using AcsExam.Core.Services;

namespace AcsExam.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IFileSystem, FileSystem>();

            services.AddSingleton<ILineParser, HeaderLineParser>();
            services.AddSingleton<ILineParser, CustomerLineParser>();
            services.AddSingleton<ILineParser, TransactionLineParser>();
            services.AddSingleton<ILineParser, FooterLineParser>();

            services.AddScoped<IFileProcessor, FileProcessor>();
            services.AddScoped<IReportService, ReportService>();

            return services;
        }
    }
}