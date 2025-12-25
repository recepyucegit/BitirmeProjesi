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
            new() { Name = "Admin", Description = "Sistem yöneticisi - Tüm yetkilere sahip" },
            new() { Name = "BranchManager", Description = "Şube müdürü - Mağaza yönetimi" },
            new() { Name = "Cashier", Description = "Kasiyer - Satış işlemleri" },
            new() { Name = "Accounting", Description = "Muhasebe - Gider ve finansal işlemler" },
            new() { Name = "Warehouse", Description = "Depo sorumlusu - Stok ve tedarik" },
            new() { Name = "TechnicalService", Description = "Teknik servis" }
        };
        await _context.Roles.AddRangeAsync(roles);
        await _context.SaveChangesAsync();

        // 8. Test Kullanıcıları
        var users = new List<User>
        {
            new() { Username = "admin", Email = "admin@teknoroms.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), IsActive = true },
            new() { Username = "manager", Email = "manager@teknoroms.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("manager123"), IsActive = true },
            new() { Username = "cashier", Email = "cashier@teknoroms.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("cashier123"), IsActive = true },
            new() { Username = "accounting", Email = "accounting@teknoroms.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("accounting123"), IsActive = true },
            new() { Username = "warehouse", Email = "warehouse@teknoroms.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("warehouse123"), IsActive = true },
            new() { Username = "techservice", Email = "techservice@teknoroms.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("techservice123"), IsActive = true }
        };
        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();

        // Rol atamaları
        var userRoles = new List<UserRole>
        {
            new() { UserId = users[0].Id, RoleId = roles.First(r => r.Name == "Admin").Id },
            new() { UserId = users[1].Id, RoleId = roles.First(r => r.Name == "BranchManager").Id },
            new() { UserId = users[2].Id, RoleId = roles.First(r => r.Name == "Cashier").Id },
            new() { UserId = users[3].Id, RoleId = roles.First(r => r.Name == "Accounting").Id },
            new() { UserId = users[4].Id, RoleId = roles.First(r => r.Name == "Warehouse").Id },
            new() { UserId = users[5].Id, RoleId = roles.First(r => r.Name == "TechnicalService").Id }
        };
        await _context.UserRoles.AddRangeAsync(userRoles);
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

        // 10. Satışlar
        var salesFaker = new Faker();
        var sales = new List<Sale>();

        for (int i = 0; i < 30; i++)
        {
            var customer = salesFaker.PickRandom(customers);
            var employee = salesFaker.PickRandom(employees);
            var store = salesFaker.PickRandom(stores);
            var saleDate = salesFaker.Date.Between(DateTime.Now.AddMonths(-3), DateTime.Now);

            var sale = new Sale
            {
                SaleDate = saleDate,
                CustomerId = customer.Id,
                EmployeeId = employee.Id,
                StoreId = store.Id,
                PaymentMethod = salesFaker.PickRandom(new[] { "Cash", "CreditCard", "BankTransfer" }),
                Status = salesFaker.PickRandom(new[] { "Completed", "Completed", "Completed", "Cancelled" }), // 75% tamamlanmış
                InvoiceNumber = $"INV-{DateTime.Now.Year}-{(i + 1):D5}",
                Notes = salesFaker.Random.Bool(0.3f) ? salesFaker.Lorem.Sentence() : null
            };

            // Her satış için 1-5 arası ürün ekle
            var saleDetails = new List<SaleDetail>();
            var numberOfProducts = salesFaker.Random.Number(1, 5);
            var selectedProducts = salesFaker.PickRandom(products, numberOfProducts).ToList();

            foreach (var product in selectedProducts)
            {
                var quantity = salesFaker.Random.Number(1, 3);
                var unitPrice = product.Price;
                var discountRate = salesFaker.Random.Decimal(0, 20); // %0-20 indirim
                var discountAmount = (unitPrice * quantity * discountRate) / 100;
                var totalPrice = unitPrice * quantity;
                var netPrice = totalPrice - discountAmount;

                var saleDetail = new SaleDetail
                {
                    ProductId = product.Id,
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                    DiscountRate = discountRate,
                    DiscountAmount = discountAmount,
                    TotalPrice = totalPrice,
                    NetPrice = netPrice
                };
                saleDetails.Add(saleDetail);
            }

            // Sale totals hesapla
            sale.TotalAmount = saleDetails.Sum(sd => sd.TotalPrice);
            sale.DiscountAmount = salesFaker.Random.Decimal(0, 500); // Ekstra indirim
            sale.NetAmount = sale.TotalAmount - sale.DiscountAmount - saleDetails.Sum(sd => sd.DiscountAmount);
            sale.CommissionAmount = sale.NetAmount * 0.10m; // %10 komisyon

            sale.SaleDetails = saleDetails;
            sales.Add(sale);
        }

        await _context.Sales.AddRangeAsync(sales);
        await _context.SaveChangesAsync();

        // 11. Tedarikçi Siparişleri
        var supplierTransactionFaker = new Faker();
        var supplierTransactions = new List<SupplierTransaction>();

        for (int i = 0; i < 20; i++)
        {
            var supplier = supplierTransactionFaker.PickRandom(suppliers);
            var employee = supplierTransactionFaker.PickRandom(employees);
            var orderDate = supplierTransactionFaker.Date.Between(DateTime.Now.AddMonths(-6), DateTime.Now);

            var transaction = new SupplierTransaction
            {
                SupplierId = supplier.Id,
                EmployeeId = employee.Id,
                OrderDate = orderDate,
                DeliveryDate = supplierTransactionFaker.Random.Bool(0.7f) ? orderDate.AddDays(supplierTransactionFaker.Random.Number(1, 14)) : null,
                Status = supplierTransactionFaker.PickRandom(new[] { "Delivered", "Delivered", "Ordered", "Cancelled" }), // Çoğunlukla teslim edilmiş
                OrderNumber = $"ORD-{DateTime.Now.Year}-{(i + 1):D5}",
                Notes = supplierTransactionFaker.Random.Bool(0.3f) ? supplierTransactionFaker.Lorem.Sentence() : null
            };

            // Her sipariş için 2-8 arası ürün ekle
            var transactionDetails = new List<SupplierTransactionDetail>();
            var numberOfProducts = supplierTransactionFaker.Random.Number(2, 8);
            var selectedProducts = supplierTransactionFaker.PickRandom(products, numberOfProducts).ToList();

            foreach (var product in selectedProducts)
            {
                var quantity = supplierTransactionFaker.Random.Number(10, 50); // Toplu alım
                var unitPrice = product.CostPrice > 0 ? product.CostPrice : (product.Price * 0.7m); // Alış fiyatı satış fiyatının %70'i

                var detail = new SupplierTransactionDetail
                {
                    ProductId = product.Id,
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                    TotalPrice = quantity * unitPrice
                };
                transactionDetails.Add(detail);
            }

            transaction.TotalAmount = transactionDetails.Sum(d => d.TotalPrice);
            transaction.Details = transactionDetails;
            supplierTransactions.Add(transaction);
        }

        await _context.SupplierTransactions.AddRangeAsync(supplierTransactions);
        await _context.SaveChangesAsync();

        // 12. Giderler
        var expenseFaker = new Faker();
        var expenses = new List<Expense>();

        var expenseCategories = new[] { "Utilities", "Rent", "Salaries", "Marketing", "Supplies", "Maintenance", "Transportation", "Insurance", "Other" };
        var expenseTypes = new[] { "FixedCost", "VariableCost", "Investment" };

        for (int i = 0; i < 40; i++)
        {
            var store = expenseFaker.PickRandom(stores);
            var employee = expenseFaker.Random.Bool(0.7f) ? expenseFaker.PickRandom(employees) : null;
            var expenseDate = expenseFaker.Date.Between(DateTime.Now.AddMonths(-4), DateTime.Now);
            var category = expenseFaker.PickRandom(expenseCategories);

            // Para birimi ve kur
            var currency = expenseFaker.PickRandom(new[] { "TL", "TL", "TL", "USD", "EUR" }); // Çoğunlukla TL
            var exchangeRate = currency == "TL" ? 1m :
                              currency == "USD" ? expenseFaker.Random.Decimal(28m, 32m) :
                              expenseFaker.Random.Decimal(30m, 35m);

            // Kategoriye göre tutar belirleme
            var amount = category switch
            {
                "Rent" => expenseFaker.Random.Decimal(20000, 50000),
                "Salaries" => expenseFaker.Random.Decimal(30000, 80000),
                "Utilities" => expenseFaker.Random.Decimal(3000, 10000),
                "Marketing" => expenseFaker.Random.Decimal(5000, 25000),
                "Insurance" => expenseFaker.Random.Decimal(8000, 20000),
                _ => expenseFaker.Random.Decimal(1000, 15000)
            };

            var expense = new Expense
            {
                ExpenseType = expenseFaker.PickRandom(expenseTypes),
                Description = GetExpenseDescription(category, expenseFaker),
                Amount = amount,
                Currency = currency,
                ExchangeRate = exchangeRate,
                AmountInTL = amount * exchangeRate,
                ExpenseDate = expenseDate,
                EmployeeId = employee?.Id,
                StoreId = store.Id,
                InvoiceNumber = expenseFaker.Random.Bool(0.7f) ? $"FTR-{DateTime.Now.Year}-{expenseFaker.Random.Number(1000, 9999)}" : null,
                Vendor = GetVendorName(category, expenseFaker),
                Category = category,
                PaymentMethod = expenseFaker.PickRandom(new[] { "BankTransfer", "Cash", "CreditCard", "Check" }),
                Status = expenseFaker.PickRandom(new[] { "Approved", "Approved", "Approved", "Pending", "Rejected" }), // Çoğunlukla onaylı
                ApprovedBy = expenseFaker.Random.Bool(0.8f) ? employees.First().Id : null,
                ApprovalDate = expenseFaker.Random.Bool(0.8f) ? expenseDate.AddDays(expenseFaker.Random.Number(1, 5)) : null,
                Notes = expenseFaker.Random.Bool(0.2f) ? expenseFaker.Lorem.Sentence() : null
            };

            expenses.Add(expense);
        }

        await _context.Expenses.AddRangeAsync(expenses);
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
        Console.WriteLine($"   - {sales.Count} Satış ({sales.Sum(s => s.SaleDetails?.Count ?? 0)} ürün satışı)");
        Console.WriteLine($"   - {supplierTransactions.Count} Tedarikçi Siparişi ({supplierTransactions.Sum(t => t.Details?.Count ?? 0)} ürün)");
        Console.WriteLine($"   - {expenses.Count} Gider (Toplam: {expenses.Where(e => e.Status == "Approved").Sum(e => e.AmountInTL):N2} TL)");
    }

    private static string GetExpenseDescription(string category, Faker faker)
    {
        return category switch
        {
            "Utilities" => faker.PickRandom(new[] { "Elektrik faturası", "Su faturası", "Doğalgaz faturası", "İnternet faturası" }),
            "Rent" => "Mağaza kira ödemesi",
            "Salaries" => "Çalışan maaş ödemesi",
            "Marketing" => faker.PickRandom(new[] { "Sosyal medya reklamları", "Google Ads kampanyası", "Billboard reklamı", "Promosyon malzemeleri" }),
            "Supplies" => faker.PickRandom(new[] { "Ofis malzemeleri", "Temizlik malzemeleri", "Kırtasiye alımı", "Ambalaj malzemeleri" }),
            "Maintenance" => faker.PickRandom(new[] { "Klima bakımı", "Bilgisayar tamiri", "Mağaza boyası", "Elektrik tamiri" }),
            "Transportation" => faker.PickRandom(new[] { "Kargo masrafları", "Yakıt gideri", "Araç bakımı" }),
            "Insurance" => faker.PickRandom(new[] { "İşyeri sigortası", "Mal sigortası", "Sorumluluk sigortası" }),
            _ => faker.Lorem.Sentence()
        };
    }

    private static string GetVendorName(string category, Faker faker)
    {
        return category switch
        {
            "Utilities" => faker.PickRandom(new[] { "BEDAŞ", "İGDAŞ", "İSKİ", "Türk Telekom", "Vodafone" }),
            "Rent" => "Gayrimenkul Sahibi",
            "Marketing" => faker.PickRandom(new[] { "Meta Business", "Google Ads", "Reklam Ajansı", "Basım Evi" }),
            "Supplies" => faker.PickRandom(new[] { "OfisKırtasiye A.Ş.", "Temizlik Dünyası", "Paketleme Ltd." }),
            "Maintenance" => faker.PickRandom(new[] { "Teknik Servis A.Ş.", "Bakım Onarım Ltd.", "Profesyonel Hizmet" }),
            "Transportation" => faker.PickRandom(new[] { "MNG Kargo", "Yurtiçi Kargo", "Aras Kargo", "Shell", "BP" }),
            "Insurance" => faker.PickRandom(new[] { "Anadolu Sigorta", "Allianz", "Aksigorta" }),
            _ => faker.Company.CompanyName()
        };
    }
}
