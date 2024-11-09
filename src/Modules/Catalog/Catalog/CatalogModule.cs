using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Behaviors;
using Shared.Data.Inteceptors;


namespace Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container
        // API Endpoint services
        // Application use case services


        // 3. Data - Infrastructure services
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<CatalogDbContext>((sp, option) =>
            {
                option.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                option.UseNpgsql(configuration.GetConnectionString("Database"));
            }
         );
        services.AddScoped<IDataSeeder, CatalogDataSeeder>();
        return services;
    }
    public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
    {
        // Use Data - Infrastructure services
        app.UseMigration<CatalogDbContext>();
        return app;
    }
}
