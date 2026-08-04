using Ecommerce.Core.Entities.Product;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Services;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Repositories;
using Ecommerce.Infrastructure.Repositories.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection infrastructureConfiguration(this IServiceCollection services,IConfiguration configuration) {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            ///applay Unit OF Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
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
