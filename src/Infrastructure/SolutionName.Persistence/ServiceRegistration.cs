using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using SolutionName.Application;
using SolutionName.Application.Abstractions.Repositories.Dapper;
using SolutionName.Application.Abstractions.Repositories.EntityFramework;
using SolutionName.Domain.Entities.Identity;
using SolutionName.Persistence.Contexts;
using SolutionName.Persistence.Repositories.Dapper;
using SolutionName.Persistence.Repositories.EntityFramework;

namespace SolutionName.Persistence
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<EFDbContext>(options => options.UseNpgsql(Configuration.ConnectionString));
            services.AddIdentityCore<UserEntity>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<EFDbContext>()
            .AddDefaultTokenProviders();
           
            services.AddScoped(typeof(IReadRepository<,>), typeof(EFReadRepository<,>));
            services.AddScoped(typeof(IRepository<,>), typeof(EFRepository<,>));
            services.AddScoped(typeof(IDapperRepository<>), typeof(DapperRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.Scan(scan => scan.FromAssemblies(
              typeof(IApplicationAssemblyMarker).Assembly
            )
            .AddClasses(@class =>
               @class.Where(type =>
                    !type.Name.StartsWith('I')
                    && type.Name.EndsWith("Repository")
               )
            )
           .UsingRegistrationStrategy(RegistrationStrategy.Skip)
           .AsImplementedInterfaces()
           .WithScopedLifetime());


            return services;
        }
    }
}
