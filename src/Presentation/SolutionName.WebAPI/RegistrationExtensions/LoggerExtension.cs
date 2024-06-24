using NpgsqlTypes;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using SolutionName.WebAPI.Configurations;

namespace SolutionName.WebAPI.RegistrationExtensions
{
    public static class LoggerExtension
    {
        public static Serilog.Core.Logger SetSeriLogConfiguration(WebApplicationBuilder builder)
        {
            var log = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt")
                .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSQL"), "logs",
                    needAutoCreateTable: true,
                    columnOptions: new Dictionary<string, ColumnWriterBase>
                    {
                        {"message", new RenderedMessageColumnWriter(NpgsqlDbType.Text)},
                        {"message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text)},
                        {"level", new LevelColumnWriter(true , NpgsqlDbType.Varchar)},
                        {"time_stamp", new TimestampColumnWriter(NpgsqlDbType.Timestamp)},
                        {"exception", new ExceptionColumnWriter(NpgsqlDbType.Text)},
                        {"log_event", new LogEventSerializedColumnWriter(NpgsqlDbType.Json)},
                        {"user_name", new UsernameColumnWriter()}
                    })
                .WriteTo.Seq(builder.Configuration["Seq:ServerURL"])
                .Enrich.FromLogContext()
                .MinimumLevel.Information()
                .CreateLogger();

            return log;
        }
    }
}
