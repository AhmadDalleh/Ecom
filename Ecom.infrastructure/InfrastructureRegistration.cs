using Ecom.Core.Interfaces;
using Ecom.infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection infrastructureConfiguration(this IServiceCollection services) 
        {
            //services.AddTransient    // i use this when i don't have any save process just like email service or sender 
            //services.AddScoped       // i use this when i have limited time save just like http request in AppDbContext 
            //service.AddSingleton    // i use this for long time save 
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            return services;
        }
    }
}
