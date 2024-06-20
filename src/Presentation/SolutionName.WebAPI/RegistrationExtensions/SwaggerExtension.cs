using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace SolutionName.WebAPI.RegistrationExtensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
          
            services.AddSwaggerGen(options =>
            {
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                options.SwaggerDoc("v1", new OpenApiInfo { Title = Assembly.GetExecutingAssembly().GetName().Name, Version = "v1", Description = "App Rest Services" });
                //options.DocumentFilter<SwaggerDocumentFilter>();
                Dictionary<string, IEnumerable<string>> security = new()
            {
                { "Bearer", Array.Empty<string>() },
            };

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.",
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
            {
              new OpenApiSecurityScheme
              {
                Reference = new OpenApiReference
                  {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                  },
                  Scheme = "Bearer",
                  Name = "Authorization",
                  In = ParameterLocation.Header,
                },
                new List<string>()
              }
            });


                options.CustomSchemaIds(x => x.FullName);
            });

            services.ConfigureOptions<ConfigureSwaggerOptions>();

            return services;
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),  
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"WebApi {description.GroupName.ToUpperInvariant()}");

                options.DefaultModelExpandDepth(2);
                options.DefaultModelRendering(ModelRendering.Model);
                options.DefaultModelsExpandDepth(-1);
                options.DisplayOperationId();
                options.DisplayRequestDuration();
                options.DocExpansion(DocExpansion.List);
                options.EnableDeepLinking();
                options.EnableFilter();
                options.MaxDisplayedTags(5);
                options.ShowExtensions();
                options.ShowCommonExtensions();
                options.EnableValidator();
            });

            return app;
        }

        public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
        {
            private readonly IApiVersionDescriptionProvider provider;

            public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
            {
                this.provider = provider;
            }

            public void Configure(SwaggerGenOptions options)
            {
                // add swagger document for every API version discovered
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
                }
            }

            public void Configure(string name, SwaggerGenOptions options)
            {
                Configure(options);
            }

            private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
            {
                var info = new OpenApiInfo()
                {
                    Title = $"Web Api Documentation [{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}] {description.ApiVersion}",
                    Version = description.ApiVersion.ToString()
                };

                if (description.IsDeprecated)
                {
                    info.Description += " This API version has been deprecated.";
                }

                return info;
            }
        }
    }
}
