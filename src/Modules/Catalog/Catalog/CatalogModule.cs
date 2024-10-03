using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container
        // API Endpoint services
        // Application use case services
        // Infrastructure services
        services.AddDbContext<CatalogDbContext>(option =>
            {
                // option.AddInterceptors(new AuditableEntityInterceptor());
                option.UseNpgsql(configuration.GetConnectionString("Database"));
            }
         );
        // services.AddScoped<IDataSeeder, CatalogDataSeeder>();
        return services;
    }
    public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
    {
        // Use Data - Infrastructure services
        app.UseMigration<CatalogDbContext>();
        return app;
    }
}
