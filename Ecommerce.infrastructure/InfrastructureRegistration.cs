using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;
namespace Ecommerce.Infrastructure
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection infrastructureConfiguration(this IServiceCollection services,IConfiguration configuration) {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            ///applay Unit OF Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // Redis Cache
            services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
            {
                var redisConnectionString =
                    configuration.GetConnectionString("Redis");

                var redisOptions =
                    ConfigurationOptions.Parse(redisConnectionString!, true);

                return ConnectionMultiplexer.Connect(redisOptions);
            });

            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(Directory.GetCurrentDirectory())
            );            ///applay Db Context
            services.AddDbContext<AppDbContext>(op => {
                op.UseSqlServer(configuration.GetConnectionString("EcommerceDB"));
            });
            return services;
        }
    }
}
