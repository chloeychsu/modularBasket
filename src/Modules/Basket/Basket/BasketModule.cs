using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data;
using Shared.Data.Inteceptors;

namespace Basket;

public static class BasketModule
{
    public static IServiceCollection AddBasketModule(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Api Endpoint services
        // 2. Application Use Case services
        // 3. Data - Infrastructure services
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<BasketDbContext>((sp, option) =>
            {
                option.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                option.UseNpgsql(configuration.GetConnectionString("Database"));
            }
        );

        return services;
    }
    public static IApplicationBuilder UseBasketModule(this IApplicationBuilder app)
    {
        // 1. Use Api Endpoint services
        // 2. Use Application Use Case services
        // 3. Use Data - Infrastructure services
        app.UseMigration<BasketDbContext>();
        return app;
    }
}
