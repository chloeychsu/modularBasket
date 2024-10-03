
namespace Catalog.Data.Seed;

public class CatalogDataSeeder(CatalogDbContext dbContext)  : IDataSeeder
{
    public async Task SeedAllAsync()
    {
        if(await dbContext.Products.AnyAsync()) return;
        await dbContext.Products.AddRangeAsync(InitialData.Products);
        await dbContext.SaveChangesAsync();
    }
}
