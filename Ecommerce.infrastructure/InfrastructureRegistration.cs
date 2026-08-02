using Ecommerce.Core.Entities.Product;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            ///applay Db Context
            services.AddDbContext<AppDbContext>(op => {
                op.UseSqlServer(configuration.GetConnectionString("EcommerceDB"));
            });
            return services;
        }
    }
}
