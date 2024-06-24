using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using SolutionName.Application.Abstractions.Services.Authentication;
using SolutionName.Application.Abstractions.Services.Authorization;
using SolutionName.Application.Concretes.Authentication;
using SolutionName.Application.Mappers;
using SolutionName.Application.Services.Authentication;
using SolutionName.Application.Services.Authorization;

namespace SolutionName.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(conf => conf.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            // AutoMapper
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>(), AppDomain.CurrentDomain.GetAssemblies());

            services.Scan(scan => scan.FromAssemblies(
               typeof(IApplicationAssemblyMarker).Assembly
            )
           .AddClasses(@class =>
             @class.Where(type =>
                   !type.Name.StartsWith('I')
                   && type.Name.EndsWith("Service")
             )
           )
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

            return services;
        }
    }
}
