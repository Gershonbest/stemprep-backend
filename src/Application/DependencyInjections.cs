using Application.Common.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;


namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            Assembly assembly = typeof(DependencyInjection).Assembly;

            services.AddAutoMapper(typeof(MappingProfile));
            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));

            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration.GetConnectionString("redisConnection")));
        
            services.AddScoped<Result>();
            services.AddLogging();
            
            return services;
        }
    }
}