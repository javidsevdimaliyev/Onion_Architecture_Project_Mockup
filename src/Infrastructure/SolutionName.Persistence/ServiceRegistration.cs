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
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<EFDbContext>(options => options.UseNpgsql(Configuration.ConnectionString));
            services.AddIdentityCore<UserEntity>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<EFDbContext>();
           
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
            //services.AddIdentity<AppUser, AppRole>(options =>
            //{
            //    options.Password.RequiredLength = 3;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequireDigit = false;
            //    options.Password.RequireLowercase = false;
            //    options.Password.RequireUppercase = false;
            //}).AddEntityFrameworkStores<ETicaretAPIDbContext>()
            //.AddDefaultTokenProviders();

            //services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
            //services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();
            //services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            //services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
            //services.AddScoped<IProductReadRepository, ProductReadRepository>();
            //services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
            //services.AddScoped<IFileReadRepository, FileReadRepository>();
            //services.AddScoped<IFileWriteRepository, FileWriteRepository>();
            //services.AddScoped<IProductImageFileReadRepository, ProductImageFileReadRepository>();
            //services.AddScoped<IProductImageFileWriteRepository, ProductImageFileWriteRepository>();
            //services.AddScoped<IInvoiceFileReadRepository, InvoiceFileReadRepository>();
            //services.AddScoped<IInvoiceFileWriteRepository, InvoiceFileWriteRepository>();
            //services.AddScoped<IBasketItemReadRepository, BasketItemReadRepository>();
            //services.AddScoped<IBasketItemWriteRepository, BasketItemWriteRepository>();
            //services.AddScoped<IBasketReadRepository, BasketReadRepository>();
            //services.AddScoped<IBasketWriteRepository, BasketWriteRepository>();
            //services.AddScoped<ICompletedOrderReadRepository, CompletedOrderReadRepository>();
            //services.AddScoped<ICompletedOrderWriteRepository, CompletedOrderWriteRepository>();
            //services.AddScoped<IEndpointReadRepository, EndpointReadRepository>();
            //services.AddScoped<IEndpointWriteRepository, EndpointWriteRepository>();
            //services.AddScoped<IMenuReadRepository, MenuReadRepository>();
            //services.AddScoped<IMenuWriteRepository, MenuWriteRepository>();
   
        }
    }
}
