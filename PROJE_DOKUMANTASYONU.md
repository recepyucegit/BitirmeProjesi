# TeknoRoma Projesi - Detaylı Teknik Dokümantasyon

## 📋 İçindekiler
1. [Proje Hakkında](#proje-hakkında)
2. [Mimari Yapı (Onion Architecture)](#mimari-yapı)
3. [Katmanlar ve Sorumlulukları](#katmanlar-ve-sorumlulukları)
4. [Design Pattern'ler ve Kullanım Amaçları](#design-patternler)
5. [Mevcut Entity'ler ve İlişkileri](#mevcut-entityler)
6. [Proje Gereksinimleri](#proje-gereksinimleri)
7. [Yapılacaklar Listesi](#yapılacaklar-listesi)
8. [Teknoloji Stack](#teknoloji-stack)

---

## 🎯 Proje Hakkında

**TeknoRoma**, bir elektronik satış mağazası yönetim sistemidir. Proje, eski bir sistemi modern teknolojilerle yeniden yazma amacıyla geliştirilmektedir.

### Ana İş Alanları:
- **Satış Yönetimi**: Müşterilere ürün satışı, faturalama
- **Stok Yönetimi**: Ürün stoklarını takip, kritik stok uyarıları
- **Personel Yönetimi**: Çalışan performansı, komisyon hesaplama, quota takibi
- **Tedarikçi Yönetimi**: Tedarikçilerden ürün siparişi
- **Müşteri Yönetimi**: Müşteri bilgileri, satış geçmişi
- **Gider Yönetimi**: Personel ödemeleri, faturalar, altyapı giderleri
- **Raporlama**: Satış, stok, gider raporları

---

## 🏛️ Mimari Yapı (Onion Architecture)

### Neden Onion Architecture?

**Geleneksel N-Tier Architecture'dan Farkı:**

```
❌ Geleneksel N-Tier (3-Katmanlı Mimari):
UI Layer → Business Logic Layer → Data Access Layer → Database

Problem: Veritabanına bağımlılık var. EntityFramework değişirse tüm katmanlar etkilenir.
```

```
✅ Onion Architecture:
UI → Application → Infrastructure → Domain (Merkez)

Avantaj: İş mantığı (Domain) hiçbir şeye bağımlı değil.
Database, UI, Framework değişse bile Domain etkilenmez.
```

### Temel Prensipler:

1. **Dependency Inversion**: Dış katmanlar içe bağımlı, iç katmanlar dışa bağımlı DEĞİL
2. **Domain Centric**: İş mantığı merkezde, teknoloji detayları dışta
3. **Testability**: Her katman bağımsız test edilebilir
4. **Maintainability**: Teknoloji değişikliği kolay

---

## 📦 Katmanlar ve Sorumlulukları

### 1️⃣ Domain Layer (TeknoRoma.Domain)

**Sorumluluğu:** Sistemin kalbi, iş kurallarını içerir

**İçeriği:**
- **Entities**: Veritabanı tablolarını temsil eden sınıflar
  ```csharp
  // Örnek: Product.cs
  public class Product : BaseEntity
  {
      public string Name { get; set; }
      public decimal Price { get; set; }
      public int StockQuantity { get; set; }
  }
  ```
- **BaseEntity**: Tüm entity'lerin ortak özellikleri
  ```csharp
  public abstract class BaseEntity
  {
      public int Id { get; set; }
      public DateTime CreatedDate { get; set; }
      public DateTime? UpdatedDate { get; set; }
      public bool IsDeleted { get; set; }  // Soft Delete için
  }
  ```

**Önemli:** Bu katman HİÇBİR KATMANA BAĞIMLI DEĞİL! EntityFramework, ASP.NET, SQL Server bilmiyor.

---

### 2️⃣ Application Layer (TeknoRoma.Application)

**Sorumluluğu:** Uygulama iş akışlarını yönetir, ne yapılacağını tanımlar

**İçeriği:**

#### A) DTOs (Data Transfer Objects)
**Neden Gerekli?**
```csharp
❌ Kötü Yaklaşım: Entity'yi direkt API'den dön
public Product GetProduct(int id)
{
    return _repository.GetById(id); // Tüm internal bilgiler gidiyor!
}

✅ İyi Yaklaşım: DTO kullan
public ProductDto GetProduct(int id)
{
    var product = _repository.GetById(id);
    return MapToDto(product); // Sadece gerekli alanları gönder
}
```

**Üç Tip DTO:**
```csharp
// 1. Read DTO - API'den data dönerken
public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

// 2. Create DTO - Yeni kayıt oluştururken
public class CreateProductDto
{
    [Required]
    public string Name { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }
}

// 3. Update DTO - Güncelleme için
public class UpdateProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```

#### B) Interfaces (Sözleşmeler)
**Dependency Inversion için kritik!**

```csharp
// Repository Interface - Ne yapılacağını tanımla
public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetActiveProductsAsync();
    Task<Product?> GetProductByBarcodeAsync(string barcode);
}

// Service Interface - İş mantığı sözleşmesi
public interface IProductService
{
    Task<ProductDto> CreateProductAsync(CreateProductDto dto);
    Task<IEnumerable<ProductDto>> GetLowStockProductsAsync();
}
```

---

### 3️⃣ Infrastructure Layer (TeknoRoma.Infrastructure)

**Sorumluluğu:** Teknik implementasyonlar, "nasıl yapılacağı"

**İçeriği:**

#### A) Repositories (Veri Erişim Katmanı)
```csharp
// Generic Repository - Ortak CRUD işlemleri
public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _context;

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>()
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }
}

// Specific Repository - Özel metodlar
public class ProductRepository : Repository<Product>, IProductRepository
{
    public async Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        return await _context.Products
            .Where(p => p.IsActive && !p.IsDeleted)
            .ToListAsync();
    }
}
```

#### B) Services (İş Mantığı Katmanı)
```csharp
public class SaleService : ISaleService
{
    private readonly IUnitOfWork _unitOfWork;

    public async Task<SaleDto> CreateSaleAsync(CreateSaleDto dto)
    {
        // 1. Validasyon
        var employee = await _unitOfWork.Employees.GetByIdAsync(dto.EmployeeId);
        if (employee == null)
            throw new KeyNotFoundException("Employee not found");

        // 2. İş Kuralı: Komisyon hesaplama
        var netAmount = totalAmount - dto.DiscountAmount;
        var commission = netAmount * employee.CommissionRate; // 10%

        // 3. Entity oluştur
        var sale = new Sale
        {
            NetAmount = netAmount,
            CommissionAmount = commission,
            // ...
        };

        // 4. Kaydet
        await _unitOfWork.Sales.AddAsync(sale);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(sale);
    }
}
```

#### C) Data (Veritabanı Katmanı)
```csharp
// DbContext - EF Core için
public class ApplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Sale> Sales { get; set; }

    // Soft Delete Otomasyonu
    public override async Task<int> SaveChangesAsync()
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.Now;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true; // Fiziksel silme YOK!
                    break;
            }
        }
        return await base.SaveChangesAsync();
    }
}

// Configuration - Fluent API
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(p => p.Barcode)
            .IsUnique();

        // Soft Delete Filter
        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
```

---

### 4️⃣ API Layer (TeknoRoma.API)

**Sorumluluğu:** HTTP isteklerini karşılar, dış dünya ile iletişim

```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(
        [FromBody] CreateProductDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var product = await _productService.CreateProductAsync(dto);
        return CreatedAtAction(nameof(GetProduct),
            new { id = product.Id }, product);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
            return NotFound();
        return Ok(product);
    }
}
```

---

## 🎨 Design Pattern'ler ve Kullanım Amaçları

### 1. Repository Pattern

**Problem:** Veritabanı sorgularını Controller'da yazmak

```csharp
❌ Kötü:
public class ProductController
{
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _context.Products
            .Where(p => !p.IsDeleted)
            .Include(p => p.Category)
            .ToListAsync(); // Controller veritabanı bilgisine sahip!
        return Ok(products);
    }
}

✅ İyi:
public class ProductController
{
    private readonly IProductService _service;

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _service.GetAllAsync();
        return Ok(products); // Controller sadece HTTP'den sorumlu
    }
}
```

**Avantajlar:**
- Veri erişim mantığı tek yerde
- Test edilebilir (mock'lanabilir)
- Veritabanı değişirse sadece repository değişir

---

### 2. Unit of Work Pattern

**Problem:** Her repository kendi SaveChanges yapıyor

```csharp
❌ Kötü:
public async Task CreateSaleAsync()
{
    await _saleRepository.AddAsync(sale);
    await _saleRepository.SaveChangesAsync(); // ✓ Sale kaydedildi

    await _productRepository.UpdateStockAsync(product);
    await _productRepository.SaveChangesAsync();
    // ❌ Hata! Sale kaydedildi ama stok güncellenemedi!

    // Sonuç: Veritabanı tutarsız!
}

✅ İyi - Unit of Work:
public async Task CreateSaleAsync()
{
    await _unitOfWork.Sales.AddAsync(sale);
    await _unitOfWork.Products.UpdateStockAsync(product);

    await _unitOfWork.SaveChangesAsync();
    // İkisi birden kaydedilir veya hiçbiri!
}
```

**Implementation:**
```csharp
public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    ISaleRepository Sales { get; }
    ICustomerRepository Customers { get; }

    Task<int> SaveChangesAsync(); // Tek kaydetme noktası!
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IProductRepository Products { get; }
    public ISaleRepository Sales { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Products = new ProductRepository(_context); // Aynı context!
        Sales = new SaleRepository(_context);       // Aynı context!
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync(); // Tek transaction!
    }
}
```

**Avantajlar:**
- Transaction yönetimi
- Veri tutarlılığı
- Atomic işlemler (hepsi ya da hiçbiri)

---

### 3. Dependency Injection

**Problem:** Sınıflar birbirine bağımlı

```csharp
❌ Kötü - Tight Coupling:
public class ProductService
{
    private readonly ProductRepository _repository;

    public ProductService()
    {
        _repository = new ProductRepository(); // Sıkı bağımlılık!
    }
}

✅ İyi - Loose Coupling:
public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository; // Interface'e bağımlı
    }
}

// Startup.cs - ServiceRegistration
services.AddScoped<IProductRepository, ProductRepository>();
services.AddScoped<IProductService, ProductService>();
```

**Avantajlar:**
- Test edilebilir (mock repository kullanabilirsin)
- Değişime açık (farklı implementation kullanabilirsin)
- SOLID prensiplerini destekler

---

### 4. Soft Delete Pattern

**Problem:** Verileri silince geri getirilemez

```csharp
❌ Kötü - Hard Delete:
public async Task DeleteProductAsync(int id)
{
    var product = await _context.Products.FindAsync(id);
    _context.Products.Remove(product); // Fiziksel olarak silindi! ❌
    await _context.SaveChangesAsync();
}

✅ İyi - Soft Delete:
public async Task DeleteProductAsync(int id)
{
    var product = await _context.Products.FindAsync(id);
    product.IsDeleted = true; // Sadece flag güncellendi ✓
    await _context.SaveChangesAsync();
}

// DbContext - Otomatik Soft Delete
public override async Task<int> SaveChangesAsync()
{
    foreach (var entry in ChangeTracker.Entries<BaseEntity>())
    {
        if (entry.State == EntityState.Deleted)
        {
            entry.State = EntityState.Modified;
            entry.Entity.IsDeleted = true; // Delete → Update'e çevir
        }
    }
    return await base.SaveChangesAsync();
}

// Query Filter - Soft delete'liler otomatik gizlenir
builder.HasQueryFilter(p => !p.IsDeleted);
```

**Avantajlar:**
- Veri kaybı yok
- Audit trail (kim ne zaman sildi?)
- Geri alınabilir

---

## 📊 Mevcut Entity'ler ve İlişkileri

### Entity İlişki Diyagramı

```
Category (1) ──────────> (*) Product
                            │
Supplier (1) ──────────> (*)│
                            │
                            └────> (*) SaleDetail
                                      │
Customer (1) ──> (*) Sale (1) ──────>│
                      │
Employee (1) ────────>│
```

### 1. Category (Kategori)
```csharp
public class Category : BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public virtual ICollection<Product>? Products { get; set; }
}
```

**Örnekler:**
- Bilgisayar ve Laptop
- Cep Telefonu ve Tablet
- Fotoğraf ve Kamera

---

### 2. Supplier (Tedarikçi)
```csharp
public class Supplier : BaseEntity
{
    public string CompanyName { get; set; }
    public string? TaxNumber { get; set; }
    public string? Phone { get; set; }
    public bool IsActive { get; set; }
    public virtual ICollection<Product>? Products { get; set; }
}
```

**Örnekler:**
- Apple Turkey
- Samsung Electronics
- Dell Turkey

---

### 3. Product (Ürün)
```csharp
public class Product : BaseEntity
{
    public string Name { get; set; }
    public string? Barcode { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int CriticalStockLevel { get; set; }
    public bool IsActive { get; set; }
    public int CategoryId { get; set; }
    public int? SupplierId { get; set; }

    public virtual Category? Category { get; set; }
    public virtual Supplier? Supplier { get; set; }
}
```

**Örnekler:**
- Dell XPS 15 (45,000 TL, Stok: 15)
- iPhone 15 Pro Max (65,000 TL, Stok: 25)
- MacBook Pro 14 (75,000 TL, Stok: 8)

**İş Kuralı:** `StockQuantity < CriticalStockLevel` → Uyarı!

---

### 4. Employee (Çalışan)
```csharp
public class Employee : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string IdentityNumber { get; set; } // TC Kimlik
    public DateTime HireDate { get; set; }
    public decimal Salary { get; set; }
    public decimal SalesQuota { get; set; } = 10000;
    public decimal CommissionRate { get; set; } = 0.10m; // %10
    public string Role { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public bool IsActive { get; set; }
}
```

**Roller:**
- Branch Manager (Şube Müdürü)
- Sales Representative (Satış Temsilcisi)
- Mobile Sales (Gezici Satış)
- Warehouse (Depo)
- Accounting (Muhasebe)
- Technical Service (Teknik Servis)

**İş Kuralı:**
```
Komisyon = NetAmount × CommissionRate
Örn: 105,000 TL × 0.10 = 10,500 TL komisyon
```

---

### 5. Customer (Müşteri)
```csharp
public class Customer : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string IdentityNumber { get; set; } // TC (Unique!)
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? City { get; set; }
    public string CustomerType { get; set; } // Individual/Corporate
    public bool IsActive { get; set; }
}
```

**Örnekler:**
- Ahmet Yilmaz (Individual, Istanbul)
- Mehmet Demir (Corporate, Izmir)

---

### 6. Sale (Satış Başlığı)
```csharp
public class Sale : BaseEntity
{
    public DateTime SaleDate { get; set; }
    public int CustomerId { get; set; }
    public int EmployeeId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal NetAmount { get; set; }
    public decimal CommissionAmount { get; set; }
    public string PaymentMethod { get; set; }
    public string Status { get; set; }
    public string InvoiceNumber { get; set; }

    public virtual Customer? Customer { get; set; }
    public virtual Employee? Employee { get; set; }
    public virtual ICollection<SaleDetail>? SaleDetails { get; set; }
}
```

**İş Kuralları:**
```
1. NetAmount = TotalAmount - DiscountAmount
2. CommissionAmount = NetAmount × Employee.CommissionRate
3. InvoiceNumber = "INV-YYYYMMDD-XXXXXX" (Unique!)
```

---

### 7. SaleDetail (Satış Detayı)
```csharp
public class SaleDetail : BaseEntity
{
    public int SaleId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountRate { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal NetPrice { get; set; }

    public virtual Sale? Sale { get; set; }
    public virtual Product? Product { get; set; }
}
```

**İş Kuralları:**
```
1. TotalPrice = Quantity × UnitPrice
2. DiscountAmount = TotalPrice × (DiscountRate / 100)
3. NetPrice = TotalPrice - DiscountAmount
```

**Örnek Satış Senaryosu:**
```json
{
  "customerId": 1,
  "employeeId": 1,
  "discountAmount": 5000,
  "paymentMethod": "CreditCard",
  "saleDetails": [
    {
      "productId": 1,
      "quantity": 1,
      "unitPrice": 45000,
      "discountRate": 0
    },
    {
      "productId": 5,
      "quantity": 1,
      "unitPrice": 65000,
      "discountRate": 5
    }
  ]
}

Hesaplamalar:
- Ürün 1: 45,000 TL (indirim yok)
- Ürün 2: 65,000 - 3,250 = 61,750 TL (%5 indirim)
- TotalAmount: 110,000 TL
- DiscountAmount: 5,000 TL (genel indirim)
- NetAmount: 105,000 TL
- CommissionAmount: 10,500 TL (105,000 × 0.10)
```

---

## 📋 Proje Gereksinimleri

### Kullanıcı Rolleri ve Yetkileri

#### 1. Şube Müdürü (Branch Manager)
✅ Tüm raporları görebilir
✅ Personel satış performansını takip edebilir
✅ Stok yönetimi yapabilir
✅ Fiyat güncelleyebilir
✅ Tedarikçi yönetimi yapabilir
✅ Tüm giderleri görebilir

#### 2. Satış Temsilcisi (Sales Representative)
✅ Satış yapabilir
✅ Müşteri kaydı oluşturabilir
✅ Kendi satışlarını görebilir
✅ Ürün stok durumunu görebilir
✅ Güncel döviz kurlarını görebilir (TCMB)
❌ Fiyat değiştiremez
❌ Diğer çalışanların satışlarını göremez

#### 3. Gezici Satış (Mobile Sales)
✅ Satış yapabilir
✅ Müşteri kaydı oluşturabilir
✅ Saha satışları
✅ Kendi satışlarını görebilir

#### 4. Depo (Warehouse)
✅ Stok giriş/çıkış işlemleri
✅ Tedarikçi sipariş takibi
✅ Kritik stok uyarıları
❌ Satış yapamaz
❌ Fiyat göremez

#### 5. Muhasebe (Accounting)
✅ Tüm satışları görebilir
✅ Gider yönetimi
✅ Personel maaş ödemeleri
✅ Tedarikçi ödemeleri
✅ Fatura yönetimi
✅ Mali raporlar

#### 6. Teknik Servis (Technical Service)
✅ Garanti takibi
✅ Müşteri şikayet yönetimi
✅ Servis kayıtları
❌ Satış yapamaz

---

### Fonksiyonel Gereksinimler

#### 1. Satış Yönetimi
- [x] Çoklu ürün satışı
- [x] İndirim uygulama (ürün bazlı ve genel)
- [x] Farklı ödeme yöntemleri
- [x] Fatura oluşturma
- [x] Komisyon hesaplama
- [ ] İade işlemleri
- [ ] Taksitli satış

#### 2. Stok Yönetimi
- [x] Ürün CRUD işlemleri
- [x] Stok takibi
- [x] Kritik stok seviyesi
- [ ] Otomatik sipariş
- [ ] Stok hareketleri raporu

#### 3. Personel Yönetimi
- [x] Çalışan CRUD
- [x] Satış performans takibi
- [x] Komisyon hesaplama
- [ ] Aylık quota takibi
- [ ] Performans raporları
- [ ] Maaş ödeme takibi

#### 4. Müşteri Yönetimi
- [x] Müşteri CRUD
- [x] TC Kimlik kontrolü
- [x] Bireysel/Kurumsal ayrımı
- [ ] Müşteri satış geçmişi
- [ ] Sadakat programı

#### 5. Tedarikçi Yönetimi
- [x] Tedarikçi CRUD
- [ ] Sipariş oluşturma
- [ ] Sipariş takibi
- [ ] Ödeme takibi
- [ ] Performans analizi

#### 6. Gider Yönetimi
- [ ] Gider kategorileri
- [ ] Gider kaydı
- [ ] Ödeme takibi
- [ ] Gider raporları

#### 7. Raporlama
- [x] Çalışan satış raporu
- [ ] Ürün satış raporu
- [ ] Stok durum raporu
- [ ] Günlük/Aylık kasa
- [ ] Tedarikçi raporu
- [ ] Gider raporu
- [ ] Kar-Zarar raporu

#### 8. Dış Entegrasyonlar
- [ ] TCMB Döviz Kuru
- [ ] E-Fatura (opsiyonel)

---

## 📝 Yapılacaklar Listesi

### ✅ Faz 1: Temel Entity'ler (TAMAMLANDI)
- [x] Category
- [x] Product
- [x] Supplier
- [x] Employee
- [x] Customer
- [x] Sale
- [x] SaleDetail

---

### 🔄 Faz 2: Kalan Entity'ler (ŞİMDİ BURАДAYIZ!)

#### A) SupplierTransaction (Tedarikçi Siparişleri)
**Süre:** 1-2 gün
**Amaç:** Tedarikçilerden alınan siparişleri takip

```csharp
public class SupplierTransaction : BaseEntity
{
    public int SupplierId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } // Ordered, Delivered, Cancelled
}

public class SupplierTransactionDetail : BaseEntity
{
    public int SupplierTransactionId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
```

#### B) Expense (Giderler)
**Süre:** 1 gün
**Amaç:** Tüm giderleri takip

```csharp
public class Expense : BaseEntity
{
    public DateTime ExpenseDate { get; set; }
    public string Category { get; set; } // Salary, Rent, etc.
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public int? EmployeeId { get; set; }
    public string Status { get; set; } // Pending, Paid
}
```

---

### 🔐 Faz 3: Authentication & Authorization (2-3 gün)

#### JWT Token Implementasyonu
```csharp
public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginDto dto);
    Task<TokenDto> RefreshTokenAsync(string refreshToken);
}

public class LoginDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class LoginResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public EmployeeDto Employee { get; set; }
}
```

#### Role-Based Authorization
```csharp
[Authorize(Roles = "Branch Manager")]
[HttpGet("all-sales")]
public async Task<IActionResult> GetAllSales() { }

[Authorize(Roles = "Sales Representative,Mobile Sales")]
[HttpPost("create-sale")]
public async Task<IActionResult> CreateSale() { }
```

---

### 🌐 Faz 4: Dış Entegrasyonlar (1 gün)

#### TCMB Döviz Kuru API
```csharp
public interface ICurrencyService
{
    Task<CurrencyRatesDto> GetCurrentRatesAsync();
    Task<decimal> GetUsdToTryAsync();
    Task<decimal> GetEurToTryAsync();
}

// API: https://www.tcmb.gov.tr/kurlar/today.xml
```

**Kullanım:**
- Dashboard'da gösterim
- Satış ekranında döviz cinsinden fiyat
- Raporlarda döviz bazlı analizler

---

### 📊 Faz 5: Dashboard & Raporlar (3-4 gün)

- Günlük/Aylık satış grafikleri
- Çalışan performans kartları
- Stok kritik seviye uyarıları
- En çok satan ürünler
- Müşteri analizi

---

### 💻 Faz 6: Frontend (2-3 hafta)

- React ile UI
- API entegrasyonu
- Responsive tasarım

---

## 🛠️ Teknoloji Stack

### Backend
- **.NET 7.0** (C#)
- **ASP.NET Core Web API**
- **Entity Framework Core 7**
- **SQL Server LocalDB**
- **JWT Bearer Authentication** (Planlı)

### Frontend (Planlı)
- **React.js**
- **Port:** 5173

### Database
- **SQL Server LocalDB**
- **Database:** TeknoRomaDb_Dev
- **Connection:** `Server=(localdb)\\MSSQLLocalDB;Database=TeknoRomaDb_Dev;Trusted_Connection=true;`

### Araçlar
- Visual Studio Code
- Swagger UI
- Git/GitHub

---

## 🚀 Önerilen İlerleme Planı

### Öncelik Sırası:

#### 1. SupplierTransaction (1-2 gün)
**Neden önce?**
- Stok yönetimi için kritik
- Sale entity'sine benzer, kolayca yapılır

#### 2. Expense (1 gün)
**Neden?**
- Gider takibi önemli
- Maaş ödemeleri için gerekli

#### 3. JWT Authentication (2-3 gün)
**Neden?**
- Tüm endpoint'ler hazır
- Frontend'den önce olmalı

#### 4. TCMB Döviz Kuru (1 gün)
**Neden?**
- Kullanıcı isteği var
- Basit HTTP request

#### 5. Dashboard & Raporlar (3-4 gün)
**Neden?**
- Tüm veri hazır
- En değerli özellik

#### 6. Frontend (2-3 hafta)
- React ile UI
- API entegrasyonu

---

## 💡 Mimari Kararlar ve Nedenleri

### 1. Neden Onion Architecture?
**Alternatif:** Clean Architecture, Hexagonal
**Seçim:**
- Domain odaklı
- Test edilebilir
- Framework bağımsız
- Uzun vadede bakımı kolay

### 2. Neden Repository + UnitOfWork?
**Alternatif:** DbContext doğrudan
**Seçim:**
- Veri erişim soyutlanmış
- Transaction yönetimi
- Test edilebilir

### 3. Neden Soft Delete?
**Alternatif:** Hard Delete
**Seçim:**
- Veri kaybı olmaz
- Audit trail
- Geri alınabilir

### 4. Neden DTO Pattern?
**Alternatif:** Entity direkt dönmek
**Seçim:**
- Güvenlik
- API versiyonlama
- Performans

---

## 📞 Özet

**Tamamlanan:**
Category, Product, Supplier, Employee, Customer, Sale, SaleDetail

**Sırada:**
SupplierTransaction → Expense → JWT Auth → TCMB → Dashboard → Frontend

**Mimari:**
Onion Architecture + Repository + UnitOfWork + Soft Delete + DTO

**Teknoloji:**
.NET 7, EF Core, SQL Server, React (planlı)
