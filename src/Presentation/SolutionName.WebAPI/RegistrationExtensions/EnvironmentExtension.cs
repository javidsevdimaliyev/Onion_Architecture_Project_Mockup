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
            builder.Configuration.AddCommandLine(Environment.GetCommandLineArgs())
                   .AddJsonFile("appsettings.json", false, true)
                   .AddJsonFile($"appsettings.{environment}.json", true);

            return builder;
        }
    }
}
