using FluentValidation;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using SolutionName.Application;
using SolutionName.Infrastructure;
using SolutionName.Persistence;
using SolutionName.WebAPI.Filters;
using SolutionName.WebAPI.Middlewares;
using SolutionName.WebAPI.RegistrationExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.SetEnvironment();

/// Add CORS policy
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins("http://localhost:5000", "https://localhost:5001")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
));

/// Serilog
Logger log = LoggerExtension.SetSeriLogConfiguration(builder);

builder.Host.UseSerilog(log);

/// Register Response Middleware
builder.Services.AddTransient<APIResponseMiddleware>();
/// Register JwtMiddleware service
builder.Services.AddTransient<JwtMiddleware>();


builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
    // options.Filters.Add<RolePermissionFilter>();
    //options.Filters.Add(typeof(GlobalApiExceptionFilter));
});
//.ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

/// Add IHttpContextAccessor 
builder.Services.AddHttpContextAccessor();

/// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

/// IDataProtectionProvider servisini ekleyin
builder.Services.AddDataProtection();

/// Api Versioning
builder.Services.AddAndConfigureApiVersioning();

/// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<IApplicationAssemblyMarker>();
builder.Services.AddEndpointsApiExplorer();

/// Add Swagger
builder.Services.AddSwagger();




var app = builder.Build();

/// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwagger(provider);
    app.UseSwaggerUI();
   
}

/// Set specific column vlaue to serilog custom column
app.UseSerilogRequestLogging();

app.Use(async (context, next) =>
{
    var username = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
    LogContext.PushProperty("user_name", username);
    await next();
});

app.UseCors();

app.UseHttpsRedirection();

app.UseMiddleware<JwtMiddleware>();

app.UseAuthorization();

app.UseMiddleware<APIResponseMiddleware>();

app.MapControllers();

app.Run();
