using Microsoft.EntityFrameworkCore;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Data.SeedData;

public static class CategorySeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Categories.AnyAsync())
            return;

        var categories = new List<Category>
        {
            new Category { Name = "Bilgisayar ve Laptop", Description = "Masaüstü bilgisayarlar, dizüstü bilgisayarlar ve aksesuarları", IsActive = true },
            new Category { Name = "Cep Telefonu ve Tablet", Description = "Akıllı telefonlar, tabletler ve mobil aksesuarlar", IsActive = true },
            new Category { Name = "Fotoğraf ve Kamera", Description = "Dijital fotoğraf makineleri, kameralar ve ekipmanları", IsActive = true },
            new Category { Name = "Bilgisayar Bileşenleri", Description = "İşlemci, ekran kartı, anakart ve diğer donanımlar", IsActive = true },
            new Category { Name = "Oyun Konsolları", Description = "PlayStation, Xbox ve Nintendo konsolları", IsActive = true },
            new Category { Name = "Akıllı Saat ve Giyilebilir Teknoloji", Description = "Smartwatch, fitness tracker ve wearable cihazlar", IsActive = true },
            new Category { Name = "Ses Sistemleri", Description = "Kulaklıklar, hoparlörler ve ses sistemleri", IsActive = true },
            new Category { Name = "Ağ Ürünleri", Description = "Modem, router, switch ve network ekipmanları", IsActive = true }
        };

        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
    }
}
