using System;

namespace SolutionName.WebAPI.RegistrationExtensions
{
    public static class EnvironmentExtension
    {
        public static WebApplicationBuilder SetEnvironment(this WebApplicationBuilder builder)
        {
            var args = Environment.GetCommandLineArgs();
            var envArg = Array.IndexOf(args, "--environment");
            var envFromArgs = envArg >= 0 ? args[envArg + 1] : null;
            var aspnetcorenv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var dotnetcorenv = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
            var environment = envFromArgs ?? (string.IsNullOrWhiteSpace(aspnetcorenv)
                ? dotnetcorenv
                : aspnetcorenv);
            Console.WriteLine($@"Current ENV :{environment}");
            builder.Configuration.AddCommandLine(args)
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                   .AddJsonFile($"appsettings.{environment}.json", optional: true)
                   .AddEnvironmentVariables();

            return builder;
        }

        public static WebApplicationBuilder AddCustomEnvironmentConf(this WebApplicationBuilder builder)
        {
            // Get command line arguments from Environment
            var args = Environment.GetCommandLineArgs();

            // Checking command line arguments
            var envArg = Array.Find(args, arg => arg.StartsWith("--env="));
            var environment = envArg != null ? envArg.Split('=')[1] : null;

            // Controlling environment variables
            var aspnetcoreEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var dotnetEnv = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
            var customEnv = Environment.GetEnvironmentVariable("Env");

            // Setting the environment variable
            environment = environment ?? aspnetcoreEnv ?? dotnetEnv ?? customEnv;

            Console.WriteLine($"Current ENV: {environment}");

            // Create IConfigurationRoot and add configuration resources
            var configuration = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            // Assign the created IConfigurationRoot to builder.Configuration
            builder.Configuration.AddConfiguration(configuration);

            //var env = builder.Configuration["Env"];
            //var dotnetEnv = builder.Configuration["DOTNET_ENVIRONMENT"];

            return builder;
        }

    }
}
