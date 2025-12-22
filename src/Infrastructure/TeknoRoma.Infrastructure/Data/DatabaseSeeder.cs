using Bogus;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Data;

public class DatabaseSeeder
{
    private readonly ApplicationDbContext _context;

    public DatabaseSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        // Zaten veri varsa seed yapma
        if (_context.Categories.Any())
        {
            return;
        }

        // 1. Kategoriler
        var categories = new List<Category>
        {
            new() { Name = "Bilgisayar", Description = "Dizüstü ve masaüstü bilgisayarlar", IsActive = true },
            new() { Name = "Telefon", Description = "Akıllı telefonlar", IsActive = true },
            new() { Name = "Tablet", Description = "Tabletler ve e-okuyucular", IsActive = true },
            new() { Name = "Aksesuar", Description = "Elektronik aksesuarlar", IsActive = true },
            new() { Name = "Ses Sistemleri", Description = "Kulaklık, hoparlör", IsActive = true },
            new() { Name = "Oyun Konsolu", Description = "Oyun konsolları ve aksesuarları", IsActive = true }
        };
        await _context.Categories.AddRangeAsync(categories);
        await _context.SaveChangesAsync();

        // 2. Tedarikçiler
        var supplierFaker = new Faker<Supplier>()
            .RuleFor(s => s.CompanyName, f => f.Company.CompanyName())
            .RuleFor(s => s.ContactName, f => f.Name.FullName())
            .RuleFor(s => s.ContactTitle, f => f.Name.JobTitle())
            .RuleFor(s => s.Email, f => f.Internet.Email())
            .RuleFor(s => s.Phone, f => f.Phone.PhoneNumber("###-###-####"))
            .RuleFor(s => s.Address, f => f.Address.FullAddress())
            .RuleFor(s => s.City, f => f.Address.City())
            .RuleFor(s => s.Country, f => "Türkiye")
            .RuleFor(s => s.PostalCode, f => f.Address.ZipCode())
            .RuleFor(s => s.TaxNumber, f => f.Random.Replace("##########"))
            .RuleFor(s => s.IsActive, f => true);

        var suppliers = supplierFaker.Generate(10);
        await _context.Suppliers.AddRangeAsync(suppliers);
        await _context.SaveChangesAsync();

        // 3. Ürünler
        var productNames = new Dictionary<string, List<string>>
        {
            ["Bilgisayar"] = new() { "MacBook Pro M3", "Dell XPS 15", "HP Pavilion", "Lenovo ThinkPad", "Asus ROG", "MSI Gaming Laptop" },
            ["Telefon"] = new() { "iPhone 15 Pro", "Samsung Galaxy S24", "Xiaomi 13 Pro", "OnePlus 11", "Google Pixel 8", "Oppo Find X6" },
            ["Tablet"] = new() { "iPad Pro", "Samsung Galaxy Tab S9", "Lenovo Tab P12", "Huawei MatePad", "Microsoft Surface Pro" },
            ["Aksesuar"] = new() { "Magic Mouse", "Logitech MX Master", "USB-C Hub", "Webcam HD", "Kablosuz Şarj Aleti" },
            ["Ses Sistemleri"] = new() { "AirPods Pro", "Sony WH-1000XM5", "JBL Flip 6", "Bose QuietComfort", "Marshall Emberton" },
            ["Oyun Konsolu"] = new() { "PlayStation 5", "Xbox Series X", "Nintendo Switch", "Steam Deck", "PS5 DualSense Kol" }
        };

        var products = new List<Product>();
        foreach (var category in categories)
        {
            var categoryProducts = productNames[category.Name];
            foreach (var productName in categoryProducts)
            {
                var faker = new Faker();
                var product = new Product
                {
                    Name = productName,
                    Description = $"{productName} - En son teknoloji",
                    Barcode = faker.Random.Replace("##########"),
                    Price = faker.Random.Decimal(500, 50000),
                    CostPrice = faker.Random.Decimal(300, 40000),
                    StockQuantity = faker.Random.Number(5, 100),
                    CriticalStockLevel = 10,
                    IsActive = true,
                    CategoryId = category.Id
                };
                products.Add(product);
            }
        }
        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();

        // 4. Mağazalar
        var stores = new List<Store>
        {
            new() { StoreName = "TeknoRoma İstanbul", StoreCode = "IST001", Phone = "0212-555-0001", Email = "istanbul@teknoroms.com", Address = "Bağdat Caddesi No:123", City = "İstanbul", IsActive = true, OpeningDate = DateTime.Now.AddYears(-2) },
            new() { StoreName = "TeknoRoma Ankara", StoreCode = "ANK001", Phone = "0312-555-0001", Email = "ankara@teknoroms.com", Address = "Tunalı Hilmi Caddesi No:45", City = "Ankara", IsActive = true, OpeningDate = DateTime.Now.AddYears(-1) },
            new() { StoreName = "TeknoRoma İzmir", StoreCode = "IZM001", Phone = "0232-555-0001", Email = "izmir@teknoroms.com", Address = "Alsancak Mahallesi No:67", City = "İzmir", IsActive = true, OpeningDate = DateTime.Now.AddMonths(-6) }
        };
        await _context.Stores.AddRangeAsync(stores);
        await _context.SaveChangesAsync();

        // 5. Departmanlar
        var departments = new List<Department>
        {
            new() { DepartmentName = "Satış", Description = "Satış departmanı", IsActive = true },
            new() { DepartmentName = "Muhasebe", Description = "Muhasebe departmanı", IsActive = true },
            new() { DepartmentName = "IT", Description = "Bilgi teknolojileri", IsActive = true },
            new() { DepartmentName = "İnsan Kaynakları", Description = "İK departmanı", IsActive = true }
        };
        await _context.Departments.AddRangeAsync(departments);
        await _context.SaveChangesAsync();

        // 6. Müşteriler
        var customerFaker = new Faker<Customer>()
            .RuleFor(c => c.FirstName, f => f.Name.FirstName())
            .RuleFor(c => c.LastName, f => f.Name.LastName())
            .RuleFor(c => c.IdentityNumber, f => f.Random.Replace("###########"))
            .RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.FirstName, c.LastName))
            .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber("###-###-####"))
            .RuleFor(c => c.Address, f => f.Address.FullAddress())
            .RuleFor(c => c.City, f => f.PickRandom(new[] { "İstanbul", "Ankara", "İzmir", "Bursa", "Antalya" }))
            .RuleFor(c => c.PostalCode, f => f.Address.ZipCode())
            .RuleFor(c => c.CustomerType, f => f.PickRandom(new[] { "Individual", "Corporate" }))
            .RuleFor(c => c.IsActive, f => true);

        var customers = customerFaker.Generate(50);
        await _context.Customers.AddRangeAsync(customers);
        await _context.SaveChangesAsync();

        // 7. Roller
        var roles = new List<Role>
        {
            new() { Name = "Admin", Description = "Sistem yöneticisi" },
            new() { Name = "Manager", Description = "Mağaza müdürü" },
            new() { Name = "Sales", Description = "Satış elemanı" },
            new() { Name = "Accountant", Description = "Muhasebeci" }
        };
        await _context.Roles.AddRangeAsync(roles);
        await _context.SaveChangesAsync();

        // 8. Admin Kullanıcı
        var adminUser = new User
        {
            Username = "admin",
            Email = "admin@teknoroms.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            IsActive = true
        };
        await _context.Users.AddAsync(adminUser);
        await _context.SaveChangesAsync();

        // Admin role assignment
        var adminUserRole = new UserRole
        {
            UserId = adminUser.Id,
            RoleId = roles.First(r => r.Name == "Admin").Id
        };
        await _context.UserRoles.AddAsync(adminUserRole);
        await _context.SaveChangesAsync();

        // 9. Çalışanlar
        var employeeFaker = new Faker<Employee>()
            .RuleFor(e => e.FirstName, f => f.Name.FirstName())
            .RuleFor(e => e.LastName, f => f.Name.LastName())
            .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.FirstName, e.LastName))
            .RuleFor(e => e.Phone, f => f.Phone.PhoneNumber("###-###-####"))
            .RuleFor(e => e.Address, f => f.Address.FullAddress())
            .RuleFor(e => e.City, f => f.PickRandom(new[] { "İstanbul", "Ankara", "İzmir" }))
            .RuleFor(e => e.IdentityNumber, f => f.Random.Replace("###########"))
            .RuleFor(e => e.HireDate, f => f.Date.Past(3))
            .RuleFor(e => e.Salary, f => f.Random.Decimal(15000, 50000))
            .RuleFor(e => e.SalesQuota, f => f.Random.Decimal(50000, 200000))
            .RuleFor(e => e.CommissionRate, f => f.Random.Decimal(0.05m, 0.15m))
            .RuleFor(e => e.Role, f => f.PickRandom(new[] { "Satış Danışmanı", "Mağaza Müdürü", "Kasa Görevlisi" }))
            .RuleFor(e => e.IsActive, f => true);

        var employees = employeeFaker.Generate(15);
        await _context.Employees.AddRangeAsync(employees);
        await _context.SaveChangesAsync();

        Console.WriteLine("✅ Seed data başarıyla oluşturuldu!");
        Console.WriteLine($"   - {categories.Count} Kategori");
        Console.WriteLine($"   - {suppliers.Count} Tedarikçi");
        Console.WriteLine($"   - {products.Count} Ürün");
        Console.WriteLine($"   - {stores.Count} Mağaza");
        Console.WriteLine($"   - {departments.Count} Departman");
        Console.WriteLine($"   - {customers.Count} Müşteri");
        Console.WriteLine($"   - {employees.Count} Çalışan");
        Console.WriteLine($"   - {roles.Count} Rol");
        Console.WriteLine($"   - 1 Admin Kullanıcı (username: admin, password: Admin123!)");
    }
}
