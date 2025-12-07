namespace TeknoRoma.Infrastructure.Data.SeedData;

public static class DataSeeder
{
    public static async Task SeedAllAsync(ApplicationDbContext context)
    {
        await CategorySeeder.SeedAsync(context);
        await ProductSeeder.SeedAsync(context);
    }
}
