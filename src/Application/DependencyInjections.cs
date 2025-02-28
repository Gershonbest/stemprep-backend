using Application.Common.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using CloudinaryDotNet;
using Application.Interfaces;

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

            //// Register Cloudinary
            //var cloudinaryConfig = configuration.GetSection("Cloudinary").Get<CloudinaryDotNet.Account>();
            //var cloudinary = new Cloudinary(cloudinaryConfig);
            //services.AddSingleton(cloudinary);
            services.AddScoped<Result>();
            services.AddLogging();
            
            return services;
        }
    }
}