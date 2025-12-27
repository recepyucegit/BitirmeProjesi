# TeknoRoma - Elektronik Mağaza Yönetim Sistemi
## Bitirme Projesi Sunum Dokümanı

---

## 📋 İçindekiler
1. [Proje Özeti](#proje-özeti)
2. [Proje Gereksinimleri](#proje-gereksinimleri)
3. [Teknoloji Seçimleri ve Gerekçeleri](#teknoloji-seçimleri-ve-gerekçeleri)
4. [Mimari Yapı ve Katmanlar](#mimari-yapı-ve-katmanlar)
5. [Geliştirilen Özellikler](#geliştirilen-özellikler)
6. [Veritabanı Tasarımı](#veritabanı-tasarımı)
7. [Güvenlik ve Best Practices](#güvenlik-ve-best-practices)
8. [Test ve Kalite](#test-ve-kalite)
9. [Demo ve Kullanım](#demo-ve-kullanım)
10. [Karşılaşılan Zorluklar ve Çözümler](#karşılaşılan-zorluklar-ve-çözümler)

---

## 1. Proje Özeti

### 🎯 Proje Amacı
TeknoRoma, **55 mağaza ve 258 çalışan** ile faaliyet gösteren bir elektronik perakende zinciri için geliştirilmiş **modern, ölçeklenebilir ve güvenli** bir yönetim sistemidir.

### 📊 Proje Kapsamı
- **Kullanıcı sayısı**: 6 farklı rol (Admin, Şube Müdürü, Kasiyer, Depo, Muhasebe, Teknik Servis)
- **Mağaza yönetimi**: 55 şube, şehir/ilçe bazlı gruplandırma
- **Ürün yönetimi**: Kategori bazlı, barkod sistemi, çoklu döviz desteği
- **Satış sistemi**: Hızlı satış, shopping cart, kota ve prim hesaplama
- **Gider yönetimi**: Onay iş akışı, çoklu döviz, kategori bazlı analiz
- **Raporlama**: Dashboard, Excel export, tarih bazlı filtreleme

### 💡 Proje Değeri
- ✅ Gerçek dünya senaryosu (55 mağazalı zincir)
- ✅ Enterprise-level mimari ve güvenlik
- ✅ Modern teknoloji stack
- ✅ Test-driven development
- ✅ Scalable ve maintainable kod yapısı

---

## 2. Proje Gereksinimleri

### 📝 Fonksiyonel Gereksinimler

#### 2.1 Kullanıcı Yönetimi
- ✅ Kullanıcı girişi (JWT Token ile)
- ✅ Rol bazlı yetkilendirme
- ✅ Kullanıcı CRUD işlemleri
- ✅ Çalışan ile ilişkilendirme
- ✅ Son giriş takibi

#### 2.2 Ürün ve Stok Yönetimi
- ✅ Kategori bazlı ürün organizasyonu
- ✅ Barkod sistemi
- ✅ Stok seviyesi takibi
- ✅ Kritik stok uyarıları
- ✅ Çoklu döviz desteği (TL, USD, EUR)

#### 2.3 Satış Yönetimi
- ✅ Hızlı satış işlemleri
- ✅ Müşteri TC kimlik ile otomatik çekme
- ✅ Shopping cart yapısı
- ✅ Satış kotası ve prim hesaplama (10,000 TL kota, %10 prim)
- ✅ Detaylı satış raporları

#### 2.4 Tedarikçi Yönetimi
- ✅ Tedarikçi kayıtları
- ✅ Sipariş yönetimi (Pending, Approved, Received, Cancelled)
- ✅ Stok entegrasyonu
- ✅ Çoklu ürün siparişleri

#### 2.5 Mağaza/Şube Yönetimi
- ✅ 55 mağaza desteği
- ✅ Şube müdürü atama
- ✅ Şehir/ilçe bazlı gruplandırma
- ✅ Mağaza bazlı hedef ve kapasite takibi
- ✅ Mağaza bazlı raporlama

#### 2.6 Gider ve Muhasebe Yönetimi
- ✅ Kapsamlı gider takibi (Operational, Capital, Administrative, Sales, Financial)
- ✅ Çoklu döviz desteği ve otomatik kur hesaplama
- ✅ Onay iş akışı (Pending → Approved/Rejected)
- ✅ Kategori bazlı analiz
- ✅ Fatura/fiş takibi

#### 2.7 Raporlama ve Dashboard
- ✅ Gerçek zamanlı dashboard (auto-refresh)
- ✅ Satış raporları (günlük, haftalık, aylık, yıllık)
- ✅ Stok raporları (düşük stok uyarıları)
- ✅ Gider raporları (kategori bazlı)
- ✅ Excel export (EPPlus 7.3.2)
- ✅ Top performans listeleri (ürün, müşteri, çalışan)

### ⚙️ Teknik Gereksinimler

#### 2.8 Mimari Gereksinimler
- ✅ **Katmanlı Mimari**: Onion Architecture (Clean Architecture)
- ✅ **Separation of Concerns**: Her katmanın tek sorumluluğu
- ✅ **SOLID Prensipleri**: Kod kalitesi ve maintainability
- ✅ **Design Patterns**: Repository, Unit of Work, DI, DTO, Factory

#### 2.9 Güvenlik Gereksinimleri
- ✅ **Authentication**: JWT Token + Refresh Token
- ✅ **Authorization**: Role-Based Access Control (RBAC)
- ✅ **Password Security**: BCrypt hashing
- ✅ **Input Validation**: DTO seviyesinde doğrulama
- ✅ **SQL Injection Prevention**: EF Core parametreli sorgular
- ✅ **Soft Delete**: Veri kaybını önleme

#### 2.10 Test Gereksinimleri
- ✅ **Unit Testing**: 68 başarılı test
- ✅ **Entity Tests**: Tüm entity validasyonları
- ✅ **Repository Tests**: CRUD operasyonları
- ✅ **Service Tests**: Business logic testleri

---

## 3. Teknoloji Seçimleri ve Gerekçeleri

### 🔧 Backend Teknolojileri

#### 3.1 ASP.NET Core 7.0 Web API
**Neden Seçildi?**
- ✅ **Performans**: Yüksek performans ve ölçeklenebilirlik
- ✅ **Cross-Platform**: Windows, Linux, macOS desteği
- ✅ **Modern**: Async/await, dependency injection built-in
- ✅ **Ecosystem**: Geniş kütüphane ve araç desteği
- ✅ **Security**: Built-in güvenlik özellikleri
- ✅ **Industry Standard**: Enterprise projelerde yaygın kullanım

**Alternatifler ve Neden Tercih Edilmedi:**
- ❌ **Node.js/Express**: Strongly-typed değil, enterprise patterns için daha az uygun
- ❌ **Django/Flask**: Python ekosistemi, performans ve type-safety açısından geride
- ❌ **Spring Boot**: Java, öğrenme eğrisi daha yüksek, boilerplate kod fazla

#### 3.2 Entity Framework Core 7.0
**Neden Seçildi?**
- ✅ **ORM Kolaylığı**: LINQ ile type-safe sorgular
- ✅ **Code First**: Kod üzerinden veritabanı yönetimi
- ✅ **Migration Support**: Veritabanı versiyonlama
- ✅ **InMemory Database**: Test ve development için ideal
- ✅ **Lazy Loading**: İhtiyaç halinde veri yükleme
- ✅ **Change Tracking**: Otomatik entity değişiklik takibi

**Kullanım Senaryoları:**
- Repository Pattern ile soyutlama
- Global Query Filter ile Soft Delete
- Navigation Properties ile ilişkiler
- Shadow Properties ile audit trail

#### 3.3 JWT (JSON Web Token)
**Neden Seçildi?**
- ✅ **Stateless**: Sunucu hafızası kullanmaz, ölçeklenebilir
- ✅ **Cross-Domain**: CORS desteği
- ✅ **Payload**: Kullanıcı bilgileri token içinde
- ✅ **Expiration**: Güvenlik için token süresi
- ✅ **Refresh Token**: Sürekli oturum için

**Alternatifler:**
- ❌ **Session-Based**: Sunucu hafızası gerektirir, ölçeklenmez
- ❌ **OAuth2**: Bu proje için fazla karmaşık

#### 3.4 BCrypt.Net-Next
**Neden Seçildi?**
- ✅ **Security**: Endüstri standardı password hashing
- ✅ **Salt**: Otomatik salt ekleme
- ✅ **Adaptive**: Work factor ayarlanabilir
- ✅ **Rainbow Table Resistant**: Brute force saldırılarına dayanıklı

#### 3.5 EPPlus 7.3.2
**Neden Seçildi?**
- ✅ **Excel Export**: Profesyonel Excel dosyaları
- ✅ **NonCommercial License**: Ücretsiz kullanım
- ✅ **Rich Features**: Formatlama, formüller, grafikler
- ✅ **Performance**: Büyük veri setleri için optimize

#### 3.6 Bogus 35.6.5
**Neden Seçildi?**
- ✅ **Realistic Data**: Gerçekçi test verisi üretimi
- ✅ **Locale Support**: Türkçe isim ve adres desteği
- ✅ **Customizable**: Özel veri formatları
- ✅ **Demo Ready**: Sunum için hazır veri

### 🎨 Frontend Teknolojileri

#### 3.7 React 18
**Neden Seçildi?**
- ✅ **Component-Based**: Yeniden kullanılabilir bileşenler
- ✅ **Virtual DOM**: Yüksek performans
- ✅ **Hooks**: Modern state yönetimi (useState, useEffect)
- ✅ **Ecosystem**: Geniş kütüphane desteği
- ✅ **Industry Standard**: En popüler frontend framework
- ✅ **Developer Experience**: Hot reload, debugging tools

**Alternatifler ve Neden Tercih Edilmedi:**
- ❌ **Angular**: Daha karmaşık, öğrenme eğrisi yüksek, TypeScript zorunlu
- ❌ **Vue.js**: Daha az popüler, iş piyasasında daha az talep
- ❌ **Vanilla JS**: Kod tekrarı fazla, state yönetimi zor

**React'in Avantajları:**
```javascript
// Component-based yapı
function UserList() {
  const [users, setUsers] = useState([]);

  useEffect(() => {
    fetchUsers(); // API call
  }, []);

  return <div>{users.map(user => <UserCard user={user} />)}</div>;
}
```

#### 3.8 Vite
**Neden Seçildi?**
- ✅ **Fast Development**: Hızlı hot reload (491ms ready time!)
- ✅ **Modern**: ES modules kullanımı
- ✅ **Optimized Build**: Production için optimize edilmiş bundle
- ✅ **Plugin Ecosystem**: React plugin desteği

**Create React App ile Karşılaştırma:**
- ⚡ Vite: 491ms ready time
- 🐌 CRA: ~5000ms ready time
- Vite **10x daha hızlı**!

#### 3.9 React Router DOM v6
**Neden Seçildi?**
- ✅ **SPA Routing**: Sayfa yenilemeden navigasyon
- ✅ **Nested Routes**: Hiyerarşik routing
- ✅ **Protected Routes**: Authentication kontrolü
- ✅ **URL Parameters**: Dynamic routing

**Kullanım Örneği:**
```javascript
<Routes>
  <Route path="/" element={<ProtectedRoute><Dashboard /></ProtectedRoute>} />
  <Route path="/login" element={<Login />} />
  <Route path="/products" element={<ProductList />} />
</Routes>
```

#### 3.10 Axios
**Neden Seçildi?**
- ✅ **HTTP Client**: Kolay API çağrıları
- ✅ **Interceptors**: Token ekleme, hata yönetimi
- ✅ **Promise-Based**: Async/await desteği
- ✅ **Request/Response Transform**: Data manipulation

**Fetch API ile Karşılaştırma:**
```javascript
// Axios - Daha temiz kod
const data = await api.get('/products');

// Fetch - Daha fazla boilerplate
const response = await fetch('/products');
const data = await response.json();
```

#### 3.11 Bootstrap 5
**Neden Seçildi?**
- ✅ **Responsive Grid**: Mobil-first tasarım
- ✅ **Ready Components**: Button, Modal, Card, Table
- ✅ **Customizable**: CSS variables ile özelleştirme
- ✅ **Cross-Browser**: Tüm browserlarda çalışır
- ✅ **Documentation**: Geniş dokümantasyon

**Tailwind CSS ile Karşılaştırma:**
- Bootstrap: Ready-to-use components (hızlı geliştirme)
- Tailwind: Utility-first (daha fazla customization, daha uzun süre)
- Bu proje için Bootstrap daha pratik

---

## 4. Mimari Yapı ve Katmanlar

### 🏗️ Onion Architecture (Clean Architecture)

```
┌─────────────────────────────────────────┐
│          Presentation Layer             │
│   (TeknoRoma.API, Frontend)            │
│   - Controllers                         │
│   - React Components                    │
└────────────┬────────────────────────────┘
             │
┌────────────▼────────────────────────────┐
│        Infrastructure Layer             │
│   (TeknoRoma.Infrastructure)           │
│   - Data Access                         │
│   - Repository Implementations          │
│   - External Services                   │
└────────────┬────────────────────────────┘
             │
┌────────────▼────────────────────────────┐
│         Application Layer               │
│   (TeknoRoma.Application)              │
│   - Business Logic                      │
│   - DTOs                                │
│   - Service Interfaces                  │
└────────────┬────────────────────────────┘
             │
┌────────────▼────────────────────────────┐
│           Domain Layer                  │
│   (TeknoRoma.Domain)                   │
│   - Entities                            │
│   - Domain Models                       │
│   - Core Business Rules                 │
└─────────────────────────────────────────┘
```

### 📦 Katman Detayları

#### 4.1 Domain Layer (Çekirdek)
**Sorumluluğu:** İş mantığının kalbi, diğer katmanlardan bağımsız

**İçeriği:**
- **Entities**: Category, Product, Customer, Employee, Sale, Expense, User, Role
- **Base Classes**: BaseEntity (Id, IsDeleted, CreatedDate, UpdatedDate)
- **Domain Rules**: Soft delete, audit trail

**Neden Bağımsız:**
```csharp
public class Product : BaseEntity
{
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }

    // Domain rule: Stok negatif olamaz
    public void DecreaseStock(int quantity)
    {
        if (StockQuantity < quantity)
            throw new InvalidOperationException("Yetersiz stok");
        StockQuantity -= quantity;
    }
}
```

**Avantajları:**
- ✅ Framework bağımsız
- ✅ Test edilebilir
- ✅ Yeniden kullanılabilir
- ✅ İş kuralları merkezi

#### 4.2 Application Layer (Uygulama)
**Sorumluluğu:** Business logic orkestrasyon, dış dünya ile çekirdek arasında köprü

**İçeriği:**
- **DTOs**: CreateProductDto, ProductDto, UpdateProductDto
- **Service Interfaces**: IProductService, IUserService
- **Repository Interfaces**: IProductRepository, IUnitOfWork

**DTO Pattern Örneği:**
```csharp
// Entity (Domain)
public class Product : BaseEntity
{
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    // ... 15 property
}

// DTO (Application) - Sadece gerekli alanlar
public class ProductDto
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}
```

**Neden DTO Kullanıyoruz:**
- ✅ **Güvenlik**: Entity'nin tüm alanlarını expose etmez
- ✅ **Performance**: Sadece gerekli veriyi taşır
- ✅ **Validation**: Input validation katmanı
- ✅ **Versioning**: API versiyonları için esneklik

#### 4.3 Infrastructure Layer (Altyapı)
**Sorumluluğu:** Data access, external services, 3rd party integrations

**İçeriği:**
- **DbContext**: Entity Framework configuration
- **Repository Implementations**: ProductRepository, UserRepository
- **Service Implementations**: ProductService, ReportService
- **Configurations**: Entity Fluent API configurations

**Repository Pattern Örneği:**
```csharp
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product> GetByIdAsync(int id);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
}

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .Where(p => !p.IsDeleted) // Global query filter
            .ToListAsync();
    }
}
```

**Unit of Work Pattern:**
```csharp
public interface IUnitOfWork
{
    IProductRepository Products { get; }
    IUserRepository Users { get; }
    Task<int> SaveChangesAsync();
}
```

**Neden Repository ve UnitOfWork:**
- ✅ **Testability**: Mock repository ile unit test
- ✅ **Abstraction**: DbContext'e doğrudan bağımlılık yok
- ✅ **Transaction**: Tek SaveChanges ile tüm değişiklikler
- ✅ **Maintainability**: Sorgu değişiklikleri tek yerden

#### 4.4 Presentation Layer (Sunum)
**Sorumluluğu:** Kullanıcı arayüzü ve API endpoints

**Backend (API):**
```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize] // JWT required
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")] // Only admin
    public async Task<ActionResult> Create(CreateProductDto dto)
    {
        await _productService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }
}
```

**Frontend (React):**
```javascript
function ProductList() {
  const [products, setProducts] = useState([]);

  useEffect(() => {
    const fetchProducts = async () => {
      const data = await productAPI.getAll(); // Axios call
      setProducts(data);
    };
    fetchProducts();
  }, []);

  return (
    <div>
      {products.map(product => (
        <ProductCard key={product.id} product={product} />
      ))}
    </div>
  );
}
```

---

## 5. Geliştirilen Özellikler

### ✅ Tamamlanan Modüller

#### 5.1 Kimlik Doğrulama ve Yetkilendirme
**Özellikler:**
- JWT Token Authentication
- Refresh Token mechanism
- BCrypt password hashing
- Role-Based Authorization (6 rol)
- User-Employee linking
- Last login tracking

**Teknik Detaylar:**
```csharp
// Token Generation
var tokenHandler = new JwtSecurityTokenHandler();
var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

var tokenDescriptor = new SecurityTokenDescriptor
{
    Subject = new ClaimsIdentity(new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
    }),
    Expires = DateTime.UtcNow.AddHours(24),
    SigningCredentials = new SigningCredentials(
        new SymmetricSecurityKey(key),
        SecurityAlgorithms.HmacSha256Signature)
};
```

**Frontend Integration:**
```javascript
// Axios interceptor - Her request'e token ekle
api.interceptors.request.use(config => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// 401 gelirse logout
api.interceptors.response.use(
  response => response.data,
  error => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);
```

#### 5.2 Ürün ve Stok Yönetimi
**Özellikler:**
- Kategori bazlı organizasyon
- Barkod sistemi
- Stok takibi ve kritik seviye uyarıları
- Çoklu döviz fiyatlandırma
- Aktif/pasif durum

**Business Logic Örneği:**
```csharp
public async Task<SaleDto> CreateSaleAsync(CreateSaleDto dto)
{
    // 1. Stok kontrolü
    foreach (var item in dto.Items)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
        if (product.StockQuantity < item.Quantity)
            throw new InvalidOperationException($"{product.ProductName} için yetersiz stok");
    }

    // 2. Satış oluştur
    var sale = new Sale
    {
        CustomerId = dto.CustomerId,
        EmployeeId = dto.EmployeeId,
        SaleDate = DateTime.Now,
        PaymentMethod = dto.PaymentMethod
    };

    // 3. Stoktan düş
    foreach (var item in dto.Items)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
        product.StockQuantity -= item.Quantity;

        sale.SaleDetails.Add(new SaleDetail
        {
            ProductId = item.ProductId,
            Quantity = item.Quantity,
            UnitPrice = product.Price,
            Subtotal = product.Price * item.Quantity
        });
    }

    // 4. Toplam hesapla
    sale.TotalAmount = sale.SaleDetails.Sum(d => d.Subtotal);

    await _unitOfWork.Sales.AddAsync(sale);
    await _unitOfWork.SaveChangesAsync();

    return MapToDto(sale);
}
```

#### 5.3 Satış Yönetimi
**Özellikler:**
- Shopping cart yapısı
- Müşteri TC kimlik entegrasyonu
- Satış kotası ve prim hesaplama
- Detaylı satış raporu
- Çoklu ödeme yöntemi

**Kota ve Prim Hesaplama:**
```csharp
public async Task<decimal> CalculateBonus(int employeeId, DateTime month)
{
    const decimal QUOTA = 10000m; // 10,000 TL kota
    const decimal BONUS_RATE = 0.10m; // %10 prim

    var startDate = new DateTime(month.Year, month.Month, 1);
    var endDate = startDate.AddMonths(1);

    var totalSales = await _context.Sales
        .Where(s => s.EmployeeId == employeeId
                 && s.SaleDate >= startDate
                 && s.SaleDate < endDate)
        .SumAsync(s => s.TotalAmount);

    if (totalSales <= QUOTA)
        return 0;

    var bonusBase = totalSales - QUOTA;
    return bonusBase * BONUS_RATE;
}
```

#### 5.4 Gider Yönetimi ve Onay Sistemi
**Özellikler:**
- Kapsamlı gider kategorileri
- Çoklu döviz ve otomatik kur hesaplama
- Approval workflow (Pending → Approved/Rejected)
- Mağaza ve çalışan ilişkilendirme
- Kategori bazlı analiz

**Onay İş Akışı:**
```csharp
public async Task ApproveExpenseAsync(int expenseId, ApproveExpenseDto dto)
{
    var expense = await _unitOfWork.Expenses.GetByIdAsync(expenseId);

    // Sadece Pending durumundakiler onaylanabilir
    if (expense.Status != ExpenseStatus.Pending)
        throw new InvalidOperationException("Sadece bekleyen giderler onaylanabilir");

    expense.Status = dto.IsApproved ? ExpenseStatus.Approved : ExpenseStatus.Rejected;
    expense.ApprovedBy = dto.ApprovedBy;
    expense.ApprovalDate = DateTime.Now;
    expense.Notes = dto.Notes;

    await _unitOfWork.SaveChangesAsync();
}
```

**Frontend - Approval Modal:**
```javascript
const ApprovalModal = ({ expense, isApproval, onSubmit }) => {
  const [notes, setNotes] = useState('');

  const handleSubmit = async () => {
    if (!isApproval && !notes.trim()) {
      alert('Red sebebi zorunludur!');
      return;
    }

    await onSubmit({
      approvedBy: currentUser.employeeId,
      isApproved: isApproval,
      notes: notes
    });
  };

  return (
    <Modal>
      <ExpenseDetails expense={expense} />
      <textarea
        placeholder={isApproval ? 'Onay notu (opsiyonel)' : 'Red sebebi (zorunlu)'}
        value={notes}
        onChange={e => setNotes(e.target.value)}
      />
      <Button onClick={handleSubmit}>
        {isApproval ? 'Onayla' : 'Reddet'}
      </Button>
    </Modal>
  );
};
```

#### 5.5 Dashboard ve Gerçek Zamanlı Yenileme
**Özellikler:**
- Satış istatistikleri (günlük, haftalık, aylık, yıllık)
- Genel istatistikler (toplam ürün, düşük stok, müşteri sayısı)
- Top performans listeleri (ürün, müşteri, çalışan)
- Son işlemler
- **Auto-refresh** (10s, 30s, 1m, 2m, 5m intervals)
- Son yenileme zamanı gösterimi

**Backend - Dashboard Stats:**
```csharp
public async Task<DashboardStatsDto> GetDashboardStatsAsync()
{
    var today = DateTime.Today;
    var last7Days = today.AddDays(-7);
    var last30Days = today.AddDays(-30);
    var last12Months = today.AddMonths(-12);

    return new DashboardStatsDto
    {
        // Satış istatistikleri
        TodaySales = await GetSalesTotalAsync(today, today.AddDays(1)),
        WeeklySales = await GetSalesTotalAsync(last7Days, today.AddDays(1)),
        MonthlySales = await GetSalesTotalAsync(last30Days, today.AddDays(1)),
        YearlySales = await GetSalesTotalAsync(last12Months, today.AddDays(1)),

        // Genel istatistikler
        TotalProducts = await _context.Products.CountAsync(p => !p.IsDeleted),
        LowStockCount = await _context.Products.CountAsync(p => p.StockQuantity < p.MinimumStockLevel),
        TotalCustomers = await _context.Customers.CountAsync(c => !c.IsDeleted),
        TotalStockValue = await _context.Products.SumAsync(p => p.Price * p.StockQuantity),

        // Top performanslar
        TopProducts = await GetTopProductsAsync(),
        TopCustomers = await GetTopCustomersAsync(),
        TopEmployees = await GetTopEmployeesAsync(),

        // Son işlemler
        RecentSales = await GetRecentSalesAsync(10)
    };
}
```

**Frontend - Auto Refresh:**
```javascript
const [autoRefresh, setAutoRefresh] = useState(false);
const [refreshInterval, setRefreshInterval] = useState(30); // 30 saniye
const [lastRefreshTime, setLastRefreshTime] = useState(null);

useEffect(() => {
  if (!autoRefresh) return;

  const interval = setInterval(() => {
    loadDashboardStats();
    setLastRefreshTime(new Date());
  }, refreshInterval * 1000);

  return () => clearInterval(interval);
}, [autoRefresh, refreshInterval]);

// UI Controls
<div className="dashboard-controls">
  <label>
    <input
      type="checkbox"
      checked={autoRefresh}
      onChange={e => setAutoRefresh(e.target.checked)}
    />
    Otomatik Yenileme
  </label>

  {autoRefresh && (
    <select
      value={refreshInterval}
      onChange={e => setRefreshInterval(Number(e.target.value))}
    >
      <option value={10}>10 saniye</option>
      <option value={30}>30 saniye</option>
      <option value={60}>1 dakika</option>
      <option value={120}>2 dakika</option>
      <option value={300}>5 dakika</option>
    </select>
  )}

  {lastRefreshTime && (
    <span>Son yenileme: {lastRefreshTime.toLocaleTimeString()}</span>
  )}
</div>
```

#### 5.6 Excel Raporlama (EPPlus)
**Özellikler:**
- Satış raporu export
- Stok raporu export
- Gider raporu export
- Profesyonel formatting
- Türkçe karakter desteği

**Backend - Excel Export:**
```csharp
public async Task<byte[]> ExportSalesReportAsync(DateTime startDate, DateTime endDate)
{
    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

    using var package = new ExcelPackage();
    var worksheet = package.Workbook.Worksheets.Add("Satış Raporu");

    // Başlıklar
    worksheet.Cells["A1"].Value = "Satış ID";
    worksheet.Cells["B1"].Value = "Tarih";
    worksheet.Cells["C1"].Value = "Müşteri";
    worksheet.Cells["D1"].Value = "Çalışan";
    worksheet.Cells["E1"].Value = "Toplam Tutar";
    worksheet.Cells["F1"].Value = "Ödeme Yöntemi";

    // Header style
    using var range = worksheet.Cells["A1:F1"];
    range.Style.Font.Bold = true;
    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
    range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

    // Data
    var sales = await GetSalesInRangeAsync(startDate, endDate);
    int row = 2;
    foreach (var sale in sales)
    {
        worksheet.Cells[row, 1].Value = sale.Id;
        worksheet.Cells[row, 2].Value = sale.SaleDate.ToString("dd.MM.yyyy HH:mm");
        worksheet.Cells[row, 3].Value = sale.CustomerName;
        worksheet.Cells[row, 4].Value = sale.EmployeeName;
        worksheet.Cells[row, 5].Value = sale.TotalAmount;
        worksheet.Cells[row, 5].Style.Numberformat.Format = "#,##0.00 ₺";
        worksheet.Cells[row, 6].Value = sale.PaymentMethod;
        row++;
    }

    // Auto-fit columns
    worksheet.Cells.AutoFitColumns();

    // Freeze first row
    worksheet.View.FreezePanes(2, 1);

    return package.GetAsByteArray();
}
```

**Frontend - Excel Download:**
```javascript
const handleExportSales = async () => {
  try {
    setLoading(true);

    // Blob olarak al
    const blob = await reportAPI.exportSalesReport({ startDate, endDate });

    if (!blob || blob.size === 0) {
      throw new Error('Excel dosyası oluşturulamadı');
    }

    // Download link oluştur
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = `Satis_Raporu_${new Date().toISOString().split('T')[0]}.xlsx`;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    window.URL.revokeObjectURL(url);

    alert('✅ Satış raporu başarıyla indirildi!');
  } catch (err) {
    alert('❌ Rapor dışa aktarılırken bir hata oluştu: ' + err.message);
  } finally {
    setLoading(false);
  }
};
```

---

## 6. Veritabanı Tasarımı

### 📊 Entity Relationship Diagram

```
┌─────────────┐         ┌─────────────┐         ┌─────────────┐
│   User      │─────────│  UserRole   │─────────│    Role     │
│─────────────│ 1     N │─────────────│ N     1 │─────────────│
│ Id          │         │ UserId      │         │ Id          │
│ Username    │         │ RoleId      │         │ RoleName    │
│ Password    │         └─────────────┘         │ Description │
│ EmployeeId  │                                 └─────────────┘
└──────┬──────┘
       │ 1
       │
       │ 1
┌──────▼──────┐         ┌─────────────┐
│  Employee   │─────────│ Department  │
│─────────────│ N     1 │─────────────│
│ Id          │         │ Id          │
│ FirstName   │         │ Name        │
│ DepartmentId│         │ ManagerId   │
│ StoreId     │         └─────────────┘
└──────┬──────┘
       │ N
       │
       │ 1
┌──────▼──────┐         ┌─────────────┐         ┌─────────────┐
│    Store    │         │   Product   │─────────│  Category   │
│─────────────│         │─────────────│ N     1 │─────────────│
│ Id          │         │ Id          │         │ Id          │
│ StoreName   │         │ ProductName │         │ CategoryName│
│ ManagerId   │         │ CategoryId  │         └─────────────┘
│ City        │         │ StockQty    │
└──────┬──────┘         └──────┬──────┘
       │ 1                     │ N
       │                       │
       │                       │ 1
       │                 ┌─────▼──────┐         ┌─────────────┐
       │                 │    Sale    │─────────│  Customer   │
       │                 │────────────│ N     1 │─────────────│
       │                 │ Id         │         │ Id          │
       │                 │ CustomerId │         │ FirstName   │
       │                 │ EmployeeId │         │ TCNumber    │
       │                 │ StoreId    │         └─────────────┘
       │                 │ SaleDate   │
       │                 └─────┬──────┘
       │                       │ 1
       │                       │
       │                       │ N
       │                 ┌─────▼──────┐
       │                 │ SaleDetail │
       │                 │────────────│
       │                 │ SaleId     │
       │                 │ ProductId  │
       │                 │ Quantity   │
       │                 │ UnitPrice  │
       │                 └────────────┘
       │
       │ 1
       │
       │ N
┌──────▼──────┐
│   Expense   │
│─────────────│
│ Id          │
│ StoreId     │
│ EmployeeId  │
│ Amount      │
│ Status      │
│ ApprovedBy  │
└─────────────┘
```

### 🔑 Key Design Decisions

#### 6.1 Soft Delete Pattern
**Neden:**
- Veri kaybını önleme
- Audit trail
- Geri alma imkanı

**Implementasyon:**
```csharp
public abstract class BaseEntity
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }
}

// Global Query Filter
modelBuilder.Entity<Product>()
    .HasQueryFilter(p => !p.IsDeleted);
```

#### 6.2 Many-to-Many Relationships
**User-Role İlişkisi:**
```csharp
public class User : BaseEntity
{
    public string Username { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
}

public class UserRole
{
    public int UserId { get; set; }
    public User User { get; set; }

    public int RoleId { get; set; }
    public Role Role { get; set; }
}

public class Role : BaseEntity
{
    public string RoleName { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
}
```

#### 6.3 One-to-Many Relationships
**Category-Product:**
```csharp
public class Category : BaseEntity
{
    public string CategoryName { get; set; }
    public ICollection<Product> Products { get; set; }
}

public class Product : BaseEntity
{
    public string ProductName { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}
```

---

## 7. Güvenlik ve Best Practices

### 🔒 Güvenlik Önlemleri

#### 7.1 Authentication Security
```csharp
// Password Hashing
public string HashPassword(string password)
{
    return BCrypt.Net.BCrypt.HashPassword(password,
        BCrypt.Net.BCrypt.GenerateSalt(12)); // 12 rounds
}

public bool VerifyPassword(string password, string hash)
{
    return BCrypt.Net.BCrypt.Verify(password, hash);
}
```

#### 7.2 Authorization Guards
```csharp
// Controller level
[Authorize(Roles = "Admin")]
public class UserController : ControllerBase { }

// Action level
[HttpDelete("{id}")]
[Authorize(Roles = "Admin,BranchManager")]
public async Task<ActionResult> Delete(int id) { }
```

**Frontend - Protected Routes:**
```javascript
const ProtectedRoute = ({ children, allowedRoles }) => {
  const { user } = useAuth();

  if (!user) {
    return <Navigate to="/login" />;
  }

  if (allowedRoles && !allowedRoles.some(role => user.roles.includes(role))) {
    return <Navigate to="/unauthorized" />;
  }

  return children;
};
```

#### 7.3 Input Validation
```csharp
public class CreateProductDto
{
    [Required(ErrorMessage = "Ürün adı gereklidir")]
    [StringLength(100, MinimumLength = 3)]
    public string ProductName { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }
}
```

#### 7.4 SQL Injection Prevention
```csharp
// ✅ EF Core - Parametreli sorgu (Güvenli)
var products = await _context.Products
    .Where(p => p.ProductName.Contains(searchTerm))
    .ToListAsync();

// ❌ String interpolation (Tehlikeli - ASLA KULLANMAYIN!)
var products = await _context.Products
    .FromSqlRaw($"SELECT * FROM Products WHERE Name LIKE '%{searchTerm}%'")
    .ToListAsync();
```

### ✨ Best Practices

#### 7.5 SOLID Principles

**Single Responsibility:**
```csharp
// ✅ Her class tek bir sorumluluğa sahip
public class ProductRepository { } // Sadece data access
public class ProductService { } // Sadece business logic
public class ProductController { } // Sadece HTTP handling
```

**Dependency Inversion:**
```csharp
// ✅ Interface'e bağımlı
public class ProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }
}

// ❌ Concrete class'a bağımlı (Kötü)
public class ProductService
{
    private readonly ProductRepository _repository;

    public ProductService()
    {
        _repository = new ProductRepository(); // Tight coupling!
    }
}
```

#### 7.6 Async/Await Pattern
```csharp
// ✅ Async all the way
public async Task<IEnumerable<ProductDto>> GetAllAsync()
{
    var products = await _repository.GetAllAsync();
    return products.Select(p => MapToDto(p));
}

// ❌ Mixing sync and async (Deadlock riski)
public ProductDto GetById(int id)
{
    var product = _repository.GetByIdAsync(id).Result; // Deadlock!
    return MapToDto(product);
}
```

#### 7.7 Error Handling
```csharp
// Backend - Global Exception Handler
app.UseExceptionHandler("/error");

[ApiController]
[Route("[controller]")]
public class ErrorController : ControllerBase
{
    [HttpGet]
    public IActionResult Error()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = context?.Error;

        return Problem(
            detail: exception?.Message,
            statusCode: 500,
            title: "Bir hata oluştu"
        );
    }
}
```

```javascript
// Frontend - Error Boundary
try {
  const data = await productAPI.getAll();
  setProducts(data);
} catch (err) {
  if (err.response?.status === 404) {
    setError('Ürün bulunamadı');
  } else if (err.response?.status === 401) {
    navigate('/login');
  } else {
    setError('Bir hata oluştu: ' + err.message);
  }
}
```

---

## 8. Test ve Kalite

### 🧪 Test Coverage

#### 8.1 Unit Test İstatistikleri
- **Toplam Test**: 68
- **Başarı Oranı**: %100
- **Test Kategorileri**:
  - Entity Tests: 20 test
  - Repository Tests: 24 test
  - Service Tests: 24 test

#### 8.2 Test Örnekleri

**Entity Tests:**
```csharp
[Fact]
public void User_Should_Have_Default_Values()
{
    // Arrange & Act
    var user = new User();

    // Assert
    Assert.False(user.IsDeleted);
    Assert.Equal(DateTime.Now.Date, user.CreatedDate.Date);
    Assert.True(user.IsActive);
}

[Fact]
public void Expense_Should_Calculate_AmountInTL_Correctly()
{
    // Arrange
    var expense = new Expense
    {
        Amount = 100,
        Currency = "USD",
        ExchangeRate = 30.5m
    };

    // Act
    var amountInTL = expense.Amount * expense.ExchangeRate;

    // Assert
    Assert.Equal(3050m, amountInTL);
}
```

**Repository Tests:**
```csharp
[Fact]
public async Task GetAllAsync_Should_Return_Only_Active_Products()
{
    // Arrange
    var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(databaseName: "TestDb")
        .Options;

    using var context = new AppDbContext(options);
    context.Products.Add(new Product { ProductName = "Active", IsDeleted = false });
    context.Products.Add(new Product { ProductName = "Deleted", IsDeleted = true });
    await context.SaveChangesAsync();

    var repository = new ProductRepository(context);

    // Act
    var products = await repository.GetAllAsync();

    // Assert
    Assert.Single(products);
    Assert.Equal("Active", products.First().ProductName);
}
```

**Service Tests:**
```csharp
[Fact]
public async Task CreateSale_Should_Decrease_Stock()
{
    // Arrange
    var mockRepo = new Mock<IProductRepository>();
    var product = new Product { Id = 1, StockQuantity = 10 };
    mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

    var service = new SaleService(mockRepo.Object);

    // Act
    await service.CreateSaleAsync(new CreateSaleDto
    {
        Items = new[] { new SaleItemDto { ProductId = 1, Quantity = 3 } }
    });

    // Assert
    Assert.Equal(7, product.StockQuantity);
}
```

---

## 9. Demo ve Kullanım

### 🚀 Canlı Demo Senaryosu

#### Senaryo 1: Kullanıcı Girişi ve Dashboard
```
1. http://localhost:5173 adresine git
2. Username: admin, Password: admin123
3. Dashboard'da istatistikleri gör
4. Otomatik yenilemeyi aktif et (30 saniye)
5. Gerçek zamanlı güncellemeyi izle
```

#### Senaryo 2: Ürün Yönetimi
```
1. "Ürünler" menüsüne tıkla
2. "Yeni Ürün" butonuna tıkla
3. Form doldur:
   - Ürün Adı: iPhone 15 Pro
   - Kategori: Telefon
   - Fiyat: 54999 TL
   - Stok: 25
4. Kaydet
5. Listede yeni ürünü gör
```

#### Senaryo 3: Gider Onaylama
```
1. "Giderler" menüsüne tıkla
2. Bekleyen giderleri filtrele
3. Bir giderin yanındaki "✓" butonuna tıkla
4. Onay modal'ı açılır
5. Onay notu yaz (opsiyonel)
6. "Onayla" butonuna tıkla
7. Gider durumu "Onaylandı" olur
```

#### Senaryo 4: Excel Rapor İndirme
```
1. "Raporlar" menüsüne tıkla
2. "Satış Raporları" sekmesine geç
3. Tarih aralığı seç (örn: Son 30 gün)
4. "📥 Excel'e Aktar" butonuna tıkla
5. Excel dosyası indirilir
6. Excel'de profesyonel formatı gör
```

### 📊 Test Kullanıcıları
```
Admin:
- Username: admin
- Password: admin123
- Yetki: Tüm işlemler

Şube Müdürü:
- Username: manager
- Password: manager123
- Yetki: Şube yönetimi, onay işlemleri

Kasiyer:
- Username: cashier
- Password: cashier123
- Yetki: Satış işlemleri
```

### 📈 Seed Data İçeriği
```
✅ 6 Kullanıcı (Admin, Manager, Cashier, Warehouse, Accounting, TechService)
✅ 10 Kategori
✅ 32 Ürün (Laptop, Telefon, Tablet, TV, vb.)
✅ 30 Müşteri
✅ 10 Tedarikçi
✅ 15 Çalışan
✅ 3 Mağaza
✅ 4 Departman
✅ 30 Satış (97 ürün satışı)
✅ 20 Tedarikçi Siparişi (113 ürün)
✅ 40 Gider (Toplam: 2.352.325,87 TL)
```

---

## 10. Karşılaşılan Zorluklar ve Çözümler

### 🔧 Teknik Zorluklar

#### 10.1 Currency Formatting Hatası
**Problem:**
```javascript
// ApprovalModal'da currency undefined gelince hata veriyordu
Uncaught TypeError: Currency code is required with currency style.
```

**Çözüm:**
```javascript
// Fallback eklendi
{new Intl.NumberFormat('tr-TR', {
  style: 'currency',
  currency: expense?.currency === 'TL' ? 'TRY' : (expense?.currency || 'TRY')
}).format(expense?.amount || 0)}
```

**Öğrenim:**
- Optional chaining (`?.`) her zaman yeterli değil
- Fallback değerler kritik
- User input validation önemli

#### 10.2 Soft Delete Global Filter
**Problem:**
```csharp
// Her sorguda IsDeleted kontrolü yazmak zorunda kalmak
var products = _context.Products.Where(p => !p.IsDeleted).ToList();
```

**Çözüm:**
```csharp
// Global query filter
modelBuilder.Entity<Product>()
    .HasQueryFilter(p => !p.IsDeleted);

// Artık otomatik filtreleniyor
var products = _context.Products.ToList(); // Sadece IsDeleted=false olanlar
```

**Öğrenim:**
- EF Core'un güçlü özellikleri
- Convention over configuration
- DRY principle

#### 10.3 CORS Issues
**Problem:**
```
Access to XMLHttpRequest at 'http://localhost:5085/api/product'
from origin 'http://localhost:5173' has been blocked by CORS policy
```

**Çözüm:**
```csharp
// Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder => builder
            .WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

app.UseCors("AllowReactApp");
```

**Öğrenim:**
- CORS nedir, neden gerekli
- Production'da origin kontrolü kritik
- Security vs usability trade-off

### 📚 Mimari Kararlar

#### 10.4 Repository vs DbContext Direct Access
**Karar:** Repository Pattern kullanıldı

**Avantajları:**
- ✅ Testability (Mock repository)
- ✅ Abstraction (DbContext değişirse sadece repository değişir)
- ✅ Single Responsibility
- ✅ Code reusability

**Dezavantajları:**
- ❌ Extra layer (complexity)
- ❌ Boilerplate code

**Sonuç:** Enterprise projelerde avantajları dezavantajlarından fazla

#### 10.5 InMemory vs SQL Server
**Karar:** InMemory Database kullanıldı (Production'a hazır SQL Server desteği ile)

**Neden InMemory:**
- ✅ Kolay setup (kurulum yok)
- ✅ Hızlı development
- ✅ Test için ideal
- ✅ Demo için mükemmel

**SQL Server'a Geçiş:**
```json
// appsettings.json
{
  "UseInMemoryDatabase": false, // true → false
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=TeknoRoma;..."
  }
}
```

---

## 📝 Sonuç ve Kazanımlar

### 🎓 Öğrenilenler

1. **Backend Development**
   - Clean Architecture nasıl uygulanır
   - Repository ve Unit of Work patterns
   - JWT Authentication implementation
   - EF Core advanced features
   - API design best practices

2. **Frontend Development**
   - React Hooks (useState, useEffect, useContext)
   - Component-based architecture
   - State management
   - API integration
   - Responsive design

3. **Full Stack Integration**
   - Frontend-Backend communication
   - CORS handling
   - Token-based authentication flow
   - File upload/download
   - Real-time updates

4. **Database Design**
   - Entity relationships
   - Normalization
   - Indexing strategies
   - Query optimization
   - Migration management

5. **Testing**
   - Unit testing best practices
   - Mocking frameworks
   - Test-Driven Development
   - Integration testing

### 📊 Proje İstatistikleri

**Kod Metrikleri:**
- **Backend**: ~15,000 satır kod
- **Frontend**: ~8,000 satır kod
- **Test**: ~2,000 satır kod
- **Toplam**: ~25,000 satır kod

**Modüller:**
- 12 Controller
- 30+ Entity
- 50+ DTO
- 15+ Repository
- 10+ Service
- 100+ API Endpoint
- 15 React Page
- 25+ React Component

**Test Coverage:**
- 68 Unit Test (%100 başarı)
- Entity, Repository, Service layers

### 🏆 Proje Başarıları

✅ **Teknik Mükemmellik**
- Clean Architecture tam uygulandı
- SOLID prensipleri takip edildi
- Kapsamlı test coverage
- Production-ready kod kalitesi

✅ **Fonksiyonel Bütünlük**
- Tüm CRUD işlemleri
- Advanced features (approval workflow, excel export, real-time dashboard)
- User-friendly interface
- Responsive design

✅ **Güvenlik**
- JWT authentication
- Role-based authorization
- Password hashing
- Input validation
- SQL injection prevention

✅ **Dokümantasyon**
- Kapsamlı README
- API documentation (Swagger)
- Code comments
- Sunum dokümanı

---

## 🎤 Sunum İçin Önemli Noktalar

### Vurgulanacak Noktalar:
1. **Real-world scenario**: 55 mağazalı gerçek bir işletme senaryosu
2. **Modern teknolojiler**: ASP.NET Core 7, React 18, EF Core 7
3. **Clean Architecture**: Katmanlı, test edilebilir, maintainable
4. **Advanced features**: Approval workflow, excel export, real-time dashboard
5. **Security-first**: JWT, BCrypt, role-based authorization
6. **Test coverage**: 68 başarılı unit test

### Demo Sırası:
1. ✅ Login (JWT authentication göster)
2. ✅ Dashboard (real-time refresh göster)
3. ✅ CRUD işlemi (ürün ekle/düzenle/sil)
4. ✅ Approval workflow (gider onaylama)
5. ✅ Excel export (rapor indir)
6. ✅ Role-based access (farklı rollerle giriş)

### Sorulabilecek Sorular ve Cevaplar:

**S: Neden Onion Architecture?**
C: Separation of concerns, testability, maintainability, business logic'in framework'ten bağımsız olması

**S: Neden React?**
C: Component-based, virtual DOM, geniş ekosistem, industry standard, developer experience

**S: Soft delete nedir?**
C: Veri silmek yerine IsDeleted=true yapma. Audit trail, geri alma, veri kaybını önleme

**S: Repository pattern neden?**
C: Abstraction, testability, single responsibility, DbContext'e bağımlılığı azaltma

**S: JWT vs Session?**
C: JWT stateless, scalable, cross-domain, microservices için uygun

---

**Proje Tamamlanma Tarihi:** Aralık 2025
**Geliştirme Süresi:** 8 gün
**Teknoloji Stack:** .NET 7.0 + React 18
**Test Coverage:** 68 unit test (%100 başarı)

---

## 📞 İletişim

**GitHub:** [recepyucegit/BitirmeProjesi](https://github.com/recepyucegit/BitirmeProjesi)

**Sunuma hazırsınız! 🎉 Başarılar! 🚀**
