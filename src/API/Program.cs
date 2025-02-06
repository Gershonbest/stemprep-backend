using Microsoft.OpenApi.Models;
using System.Text;
using API.Filters;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(configuration["AccessKey"]);
var issuer = configuration["Issuer"];
var audience = configuration["Audience"];


builder.Services.AddLogging();
builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Information()
           .WriteTo.Console()
           .CreateLogger();

Log.Information("Starting up the service");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
})
.AddCookie(IdentityConstants.ApplicationScheme);

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "StemPrep",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddAuthorization();
var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        Log.Error($"An error occurred while applying migrations: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.

app.UseSwagger(); // Make Swagger available in all environments
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stem Prep V1");
    if (!app.Environment.IsDevelopment())
    {
        c.RoutePrefix = "swagger"; // Adjust if needed for non-development environments
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(options =>
    options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());


app.UseAuthentication();
app.UseAuthorization();
 //app.UseMiddleware<AuthMiddleware>();
app.UseMiddleware<ExceptionHandlerMiddleware>();


app.MapControllers();
app.Run();
