using Microsoft.Extensions.DependencyInjection;
using ETL.Core.Interfaces;
using ETL.Core.Parsers;
using ETL.Core.Services;

namespace ETL.Core.Extensions
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