using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nutria.Domain.Interfaces;
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
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString);
        });

        //ServicesRegister
        //services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IJwtService, JwtService>();
        //services.AddScoped<IFileService, FileService>();
        //services.AddScoped<IUploadFileToAzureStorageService, UploadFileToAzureStorageService>();
        //services.AddScoped<IActivityService, ActivityService>();

        return services;
    }
}