using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Interfaces.Services;
using Nutria.Infrastructure.Persistence.Context;
using Nutria.Infrastructure.Repositories;
using Nutria.Infrastructure.Services;

namespace Nutria.Infrastructure;

public static class InfrastructureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        //Database Connection
        services.AddDbContext<AppdbContext>(options =>
        {
            // LOCAL, con con el appsettings.json

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            // DEPLOY (RAILWAY), con la DATABASE_URL
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            if (!string.IsNullOrEmpty(databaseUrl))
            {
                var uri = new Uri(databaseUrl);
                var userInfo = uri.UserInfo.Split(':');

                connectionString =
                    $"Host={uri.Host};" +
                    $"Port={uri.Port};" +
                    $"Database={uri.AbsolutePath.TrimStart('/')};" +
                    $"Username={userInfo[0]};" +
                    $"Password={userInfo[1]};" +
                    $"SSL Mode=Require;Trust Server Certificate=true";
            }

            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMealRepository, MealRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IRestaurantRepository, RestaurantRepository>();
        services.AddScoped<IHealthRepository, HealthRepository>();
        services.AddScoped<IMedicalConditionRepository, MedicalConditionRepository>();
        services.AddScoped<IIngredientRepository, IngredientRepository>();
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}