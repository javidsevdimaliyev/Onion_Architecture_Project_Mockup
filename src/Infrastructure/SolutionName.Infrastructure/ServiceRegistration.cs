using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolutionName.Application.Abstractions.Services;
using SolutionName.Application.Abstractions.Services.Storage;
using SolutionName.Application.Abstractions.Services.Storage.Local;
using SolutionName.Application.DTOs.Configurations;
using SolutionName.Infrastructure.Services;
using SolutionName.Infrastructure.Services.Storage.Local;

namespace SolutionName.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddScoped<ICacheService, MemoryCacheService>();
            services.AddFileService(configuration);
            services.AddStorage<LocalStorageService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
        }

        public static void AddStorage<T>(this IServiceCollection serviceCollection) where T : class, IStorageService
        {
            serviceCollection.AddScoped<IStorageService, T>();
        }

        public static IServiceCollection AddFileService(this IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationSection? fileSettings = configuration.GetSection("FileConfig");
            LocalStorageConfig config = new()
            {
                SaveDirectory = (string)fileSettings.GetValue<string>("SaveDirectory"),
                SaveDirectoryDebug = fileSettings.GetValue<string>("SaveDirectoryDebug")
            };

            services.AddScoped<IFileStorageService>(provider => new FileStorageService(config));
            
           
            return services;
        }
    }
}
