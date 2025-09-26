using Application.Common.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using CloudinaryDotNet;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Application.Auth.Commands;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            Assembly assembly = typeof(DependencyInjection).Assembly;

            services.AddAutoMapper(typeof(MappingProfile));
            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));

            services.AddTransient(typeof(IRequestHandler<LoginUserCommand<Parent>, Result>), typeof(LoginUserCommandHandler<Parent>));
            services.AddTransient(typeof(IRequestHandler<LoginUserCommand<Tutor>, Result>), typeof(LoginUserCommandHandler<Tutor>));
            services.AddTransient(typeof(IRequestHandler<LoginUserCommand<Admin>, Result>), typeof(LoginUserCommandHandler<Admin>));

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