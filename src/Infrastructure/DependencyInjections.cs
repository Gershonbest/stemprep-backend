using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
                   options.UseNpgsql(configuration.GetConnectionString("postgresConnection")));

            services.AddSingleton<IEmailService>(provider =>
            {
                return new EmailService(configuration);
            });
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            services.AddHttpContextAccessor();

            var accesskey = configuration["Jwt:AccessKey"];
            var refreshkey = configuration["Jwt:RefreshKey"];
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];

            services.AddScoped<ITokenGenerator, GenerateTokenService>(provider =>
            {
                return new GenerateTokenService(accesskey, refreshkey, issuer, audience);
            });

            return services;
        }
    }
}
