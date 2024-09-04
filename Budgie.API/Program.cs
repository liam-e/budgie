using System.Data.Common;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Budgie.API.Database;
using Budgie.API.Models;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        builder.Configuration.AddJsonFile($"appsettings.{environment}.json", optional: true);

        ConfigureServices(builder.Services, builder.Configuration, environment);

        var app = builder.Build();
        ConfigureMiddleware(app);
        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration, string environment)
    {
        // Register DbConnectionProvider as a singleton service
        services.AddSingleton<DbConnectionProvider>(sp =>
        {
            return new DbConnectionProvider(configuration.GetConnectionString("DefaultConnection")!);
        });

        // Register DbConnection as a scoped service
        services.AddScoped<DbConnection>(provider =>
        {
            var dbConnectionProvider = provider.GetRequiredService<DbConnectionProvider>();
            return dbConnectionProvider.GetConnection();
        });

        // Configure DbContext to use DbConnection and DbContextOptions
        services.AddDbContext<BudgetContext>((serviceProvider, options) =>
        {
            var connection = serviceProvider.GetRequiredService<DbConnection>();
            options.UseNpgsql(connection);
        });

        // Other service configurations...
        services.AddAuthentication(options =>
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
                ValidIssuer = configuration["AppSettings:Issuer"],
                ValidAudience = configuration["AppSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:SecretKey"]!)),
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Request.Cookies.TryGetValue("accessToken", out var accessToken);
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
            options => configuration.Bind("CookieSettings", options)
        );

        services.AddAuthorization();
        services.AddControllers();

        // Swagger setup
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Description = "Standard auth header",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            options.OperationFilter<SecurityRequirementsOperationFilter>();
        });
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Test")
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors(builder =>
            builder
            .WithOrigins("*/*") // .WithOrigins("http://localhost:3000")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials());

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }
}
