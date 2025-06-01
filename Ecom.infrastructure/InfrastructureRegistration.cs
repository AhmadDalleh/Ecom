using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Repositories;
using Ecom.infrastructure.Repositories.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection infrastructureConfiguration(this IServiceCollection services,IConfiguration configuration ) 
        {

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


            //Apply Unit Of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //Register email sender
            services.AddScoped<IEmailService,EmailService>();
            //Apply Redis  Connection
            //services.AddSingleton<IConnectionMultiplexer>( implementationFactory: i =>
            //{
            //    var config = ConfigurationOptions.Parse(configuration.GetConnectionString("redis"));
            //    return ConnectionMultiplexer.Connect(config);
            //});

            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            services.AddSingleton<IImageManagementService, ImageManagementService>();
            //Apply DbContext
           services.AddDbContext<AppDbContext>(op =>
            {
                op.UseSqlServer(configuration.GetConnectionString("EcomDatabase"));
            });
            return services;
        }
    }
}
