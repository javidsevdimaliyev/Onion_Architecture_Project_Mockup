using FluentValidation;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using NpgsqlTypes;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;
using SolutionName.Application;
using SolutionName.Infrastructure;
using SolutionName.Persistence;
using SolutionName.WebAPI.Configurations;
using SolutionName.WebAPI.Filters;
using SolutionName.WebAPI.Middlewares;
using SolutionName.WebAPI.RegistrationExtensions;

var builder = WebApplication.CreateBuilder(args);


#region Env operations
args = Environment.GetCommandLineArgs();
var envArg = Array.IndexOf(args, "--environment");
var envFromArgs = envArg >= 0 ? args[envArg + 1] : null;
var aspnetcore = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var dotnetcore = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
var environment = envFromArgs ?? (string.IsNullOrWhiteSpace(aspnetcore)
    ? dotnetcore
    : aspnetcore);
Console.WriteLine($@"Current ENV :{environment}");
builder.Configuration.AddCommandLine(Environment.GetCommandLineArgs())
       .AddJsonFile("appsettings.json", false, true)
       .AddJsonFile($"appsettings.{environment}.json", true);

#endregion

// Add services to the container.
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

// Add CORS policy
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins("http://localhost:5000", "https://localhost:5001").AllowAnyHeader().AllowAnyMethod().AllowCredentials()
));

//Serilog
Logger log = new LoggerConfiguration()
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

builder.Host.UseSerilog(log);

//Response Middleware
builder.Services.AddTransient<APIResponseMiddleware>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
    // options.Filters.Add<RolePermissionFilter>();
    //options.Filters.Add(typeof(GlobalApiExceptionFilter));
});
//.ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

//Api Versioning
builder.Services.AddAndConfigureApiVersioning();

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<IApplicationAssemblyMarker>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwagger(provider);
    app.UseSwaggerUI();
   
}

//Set specific column vlaue to serilog custom column
app.UseSerilogRequestLogging();

app.Use(async (context, next) =>
{
    var username = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
    LogContext.PushProperty("user_name", username);
    await next();
});


app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<APIResponseMiddleware>();

app.MapControllers();

app.Run();
