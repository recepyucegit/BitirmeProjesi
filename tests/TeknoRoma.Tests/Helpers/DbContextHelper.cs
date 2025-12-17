using Microsoft.EntityFrameworkCore;
using TeknoRoma.Infrastructure.Data;

namespace TeknoRoma.Tests.Helpers;

public static class DbContextHelper
{
    public static ApplicationDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);
        return context;
    }
}
