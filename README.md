# TeknoRoma - Elektronik Mağaza Yönetim Sistemi

## Proje Hakkında

TeknoRoma, 55 mağaza ve 258 çalışan ile Türkiye'nin önde gelen elektronik perakende zincirlerinden biri için geliştirilmiş modern bir yönetim sistemidir. Proje, tedarikçi yönetimi, stok takibi, satış işlemleri ve kapsamlı raporlama özelliklerini içerir.

## Teknoloji Stacki

### Backend
- **Framework**: ASP.NET Core 7.0 Web API
- **Mimari**: Onion Architecture (Clean Architecture)
- **Database**: Entity Framework Core InMemory Database
- **ORM**: Entity Framework Core 7.0.20
- **Authentication**: JWT Bearer Token
- **Password Hashing**: BCrypt.Net-Next 4.0.3
- **Excel Export**: EPPlus 7.3.2
- **Test Data**: Bogus 35.6.5

### Frontend
- **Framework**: React 18 + Vite
- **Routing**: React Router DOM v6
- **HTTP Client**: Axios
- **UI Library**: Bootstrap 5
- **Styling**: Custom CSS
- **Tabs**: react-tabs 6.0.2

## Mimari Yapı

Proje katmanlı mimari ile geliştirilmiştir:

### 1. Domain Layer (Core)
- **TeknoRoma.Domain**: Entity'ler ve domain modelleri
  - BaseEntity (Soft Delete desteği)
  - Category, Product, Supplier
  - Customer, Employee
  - Sale, SaleDetail
  - SupplierTransaction, SupplierTransactionDetail
  - User, Role, UserRole (Authentication & Authorization)
  - Store (Multi-Store Management)
  - Expense (Accounting & Approval Workflow)

### 2. Application Layer (Core)
- **TeknoRoma.Application**: Business logic, DTOs, Interfaces
  - Repository Interfaces
  - Service Interfaces
  - Data Transfer Objects (DTOs)

### 3. Infrastructure Layer
- **TeknoRoma.Infrastructure**: Data Access, Repository implementations
  - Entity Configurations
  - Repository Implementations
  - Service Implementations
  - Database Context (Global Query Filter ile Soft Delete)

### 4. Presentation Layer
- **TeknoRoma.API**: RESTful Web API
  - Controllers
  - Swagger Documentation
- **teknoroma-frontend**: React Web Application

## Temel Özellikler

### 1. Kullanıcı ve Kimlik Doğrulama Sistemi
- **JWT Token Authentication**: Güvenli oturum yönetimi
- **Refresh Token**: Otomatik token yenileme
- **Role-Based Authorization**: Rol bazlı yetkilendirme
- **Password Security**: BCrypt ile güvenli şifre hashleme
- **User Management**: Kullanıcı CRUD işlemleri
- **Kullanıcı Rolleri**:
  - Admin (Sistem Yöneticisi)
  - BranchManager (Şube Müdürü)
  - Cashier (Kasa Satış)
  - Warehouse (Depo Sorumlusu)
  - Accounting (Muhasebe)
  - TechnicalService (Teknik Servis)
- **User-Employee Linking**: Kullanıcı-çalışan ilişkilendirmesi
- **Activity Tracking**: Son giriş tarihi takibi

### 2. Ürün Yönetimi
- Kategori bazlı ürün organizasyonu
- Barkod sistemi
- Stok takibi
- Kritik stok seviyesi uyarıları
- Fiyat yönetimi (TL, Dolar, Euro)

### 3. Tedarikçi Yönetimi
- Tedarikçi bilgileri
- Tedarikçi hareketleri (alım, iade, ödeme)
- Tedarikçi bazlı raporlama

### 4. Satış Yönetimi
- Hızlı satış işlemleri
- Müşteri TC kimlik ile otomatik bilgi çekme
- Çoklu döviz desteği (TL, USD, EUR)
- Satış detayları ve fatura kesme
- Satış kotası ve prim hesaplama (%10 prim - 10,000 TL kota üstü)

### 5. Müşteri Yönetimi
- Müşteri kayıtları
- Satış geçmişi
- Demografik bilgiler (yaş, cinsiyet)

### 6. Çalışan Yönetimi
- Personel kayıtları
- Departman ve rol tanımlamaları
- Maaş ve prim takibi
- Şube ataması ve yönetimi

### 6.1 Şube/Mağaza Yönetimi
- **Multi-Store Support**: 55 mağaza yönetimi
- **Store Information**: Mağaza detayları (isim, kod, adres, telefon, email)
- **Manager Assignment**: Şube müdürü atama
- **Store Metrics**: Aylık hedef ve kapasite takibi
- **Opening Date Tracking**: Açılış tarihi bilgisi
- **City & District**: Şehir ve ilçe bazlı gruplandırma
- **Active/Inactive Status**: Aktif/pasif durum yönetimi
- **Store-Based Operations**:
  - Çalışan atama (Employee → Store)
  - Satış takibi (Sale → Store)
  - Gider takibi (Expense → Store)

### 7. Tedarikçi Sipariş Yönetimi
- **Comprehensive Order Management**: Kapsamlı sipariş takibi
- **Multi-Product Orders**: Çoklu ürün siparişleri
- **Stock Integration**: Otomatik stok güncellemesi
- **Order Status Tracking**:
  - Pending (Beklemede)
  - Approved (Onaylandı)
  - Received (Teslim Alındı)
  - Cancelled (İptal Edildi)
- **Payment Methods**: Çoklu ödeme yöntemleri
- **Order Details**: Ürün bazlı sipariş detayları
- **Supplier Linking**: Tedarikçi-sipariş ilişkilendirmesi

### 8. Muhasebe ve Gider Yönetimi Modülü
- **Comprehensive Expense Tracking**: Kapsamlı gider takibi
  - Operational (Operasyonel giderler)
  - Maintenance (Bakım onarım)
  - Marketing (Pazarlama)
  - Travel (Seyahat)
  - Utility (Kamu hizmetleri)
- **Multi-Currency Support**: Çoklu döviz desteği (TL, USD, EUR)
  - Otomatik kur hesaplama
  - TL'ye otomatik dönüştürme
  - Exchange rate tracking
- **Approval Workflow**: Onay iş akışı
  - Status: Pending → Approved/Rejected → Paid
  - Approver tracking (Onaylayan yönetici)
  - Approval date and notes
  - Only pending expenses can be modified
- **Expense Categorization**:
  - Category-based grouping
  - Vendor/supplier information
  - Invoice number tracking
  - Payment method (BankTransfer, Cash, CreditCard, Check)
- **Store & Employee Linking**:
  - Expense → Store (Hangi şube gideri)
  - Expense → Employee (Gider sorumlusu)
  - Expense → Approver (Onaylayan)
- **Financial Reporting**:
  - Store-based expense reports
  - Category-based expense analysis
  - Date range filtering
  - Total expenses calculation
  - Pending expenses tracking

## Raporlama Sistemi

### 📊 Dashboard (Ana Sayfa)
Kapsamlı yönetim paneli ile tüm işletme metriklerini tek ekranda görüntüleme:

**Satış İstatistikleri**
- Bugünkü satışlar
- Haftalık satışlar (Son 7 gün)
- Aylık satışlar (Son 30 gün)
- Yıllık satışlar (Son 12 ay)

**Genel İstatistikler**
- Toplam ürün sayısı
- Düşük stok uyarıları
- Toplam müşteri sayısı
- Toplam stok değeri

**Top Performanslar**
- En çok satan ürünler (Top 5)
- En iyi müşteriler (Top 5)
- En başarılı çalışanlar (Top 5)
- Düşük stok uyarıları (Kritik seviyenin altındaki ürünler)

**Son İşlemler**
- Son 10 satış kaydı
- Anlık yenileme özelliği

### 📈 Raporlar Modülü

#### 1. Satış Raporları
**Özet Bilgiler**
- Toplam satış miktarı
- Ortalama satış tutarı
- Satış adedi
- Tarih aralığı filtreleme

**Detaylı Satış Listesi**
- Satış tarihi ve saati
- Müşteri bilgileri
- Çalışan bilgileri
- Mağaza bilgileri
- Ürün detayları
- Ödeme yöntemi
- Toplam tutar

**Top Performans**
- En çok satan ürünler (Miktar ve gelir bazlı)
- En iyi müşteriler (Alışveriş sayısı ve harcama)

**Excel Export**: Tüm satış verileri Excel formatında indirilebilir

#### 2. Stok Raporları
**Genel Stok Durumu**
- Toplam ürün çeşidi
- Toplam stok değeri
- Düşük stok ürün sayısı

**Detaylı Stok Listesi**
- Ürün adı ve barkodu
- Kategori
- Mevcut stok miktarı
- Minimum stok seviyesi
- Birim fiyat
- Toplam değer
- Stok durumu (Normal/Düşük/Kritik)

**Düşük Stok Uyarıları**
- Minimum seviyenin altındaki ürünler
- Kategori bazlı gruplama

**Excel Export**: Stok raporu Excel formatında indirilebilir

#### 3. Gider Raporları
**Gider Özeti**
- Toplam gider tutarı (TL bazında)
- Bekleyen gider sayısı
- Onaylanan gider sayısı

**Detaylı Gider Listesi**
- Gider tarihi
- Kategori (Operasyonel, Bakım, Pazarlama, vb.)
- Açıklama
- Mağaza bilgisi
- Çalışan bilgisi
- Tutar ve döviz
- TL karşılığı
- Durum (Beklemede/Onaylandı/Ödendi)
- Ödeme yöntemi

**Kategori Bazlı Analiz**
- Gider kategorilerine göre dağılım
- Tarih aralığı filtreleme

**Excel Export**: Gider raporu Excel formatında indirilebilir

### 📅 Rapor Özellikleri

**Tarih Filtreleme**
- Başlangıç ve bitiş tarihi seçimi
- Varsayılan: Son 30 gün
- Özel tarih aralığı belirleme

**Excel Export Özellikleri**
- EPPlus 7.3.2 kütüphanesi ile profesyonel Excel dosyaları
- Otomatik sütun genişliği ayarlama
- Başlık satırı formatlama (Bold, Background Color)
- Freeze Pane (Başlık satırını sabitle)
- Auto Filter (Otomatik filtre)
- Türkçe karakter desteği
- Tarih ve para formatları
- NonCommercial lisans ile ücretsiz kullanım

**Performans**
- Sayfalama (Pagination) desteği
- Lazy loading
- Tarih aralığı sınırlama

## Önemli İş Kuralları

### Stok Yönetimi
- Kritik stok seviyesi kontrolü
- Stokta olmayan ürün satışı engelleme
- Otomatik uyarı sistemi

### Satış Süreçleri
- TC Kimlik ile hızlı müşteri tanıma
- Güncel döviz kuru entegrasyonu
- Satış-depo senkronizasyonu
- Kasa numarası bazlı sipariş yönetimi

### Prim Sistemi
- Satış kotası: 10,000 TL
- Prim oranı: Kota üstü satışların %10'u
- Aylık hesaplama

### Soft Delete
- Tüm silme işlemleri soft delete
- Global query filter ile otomatik filtreleme
- Veri bütünlüğü korunması

## Veritabanı

### Ana Tablolar
- Categories (Kategoriler)
- Products (Ürünler)
- Suppliers (Tedarikçiler)
- Customers (Müşteriler)
- Employees (Çalışanlar)
- Sales & SaleDetails (Satışlar)
- SupplierTransactions & SupplierTransactionDetails (Tedarikçi Hareketleri)
- Users (Kullanıcılar)
- Roles (Roller)
- UserRoles (Kullanıcı-Rol İlişkisi)
- Stores (Mağazalar/Şubeler)
- Expenses (Giderler)

### Özellikler
- Referential Integrity
- Indexing
- Soft Delete Support
- Audit Fields (CreatedDate, UpdatedDate, CreatedBy, UpdatedBy)

## Kurulum

### Gereksinimler
- **.NET 7.0 SDK** ([İndir](https://dotnet.microsoft.com/download/dotnet/7.0))
- **Node.js 18+** ve npm ([İndir](https://nodejs.org/))
- **Git** ([İndir](https://git-scm.com/))

### Projeyi İndirme
```bash
# Projeyi klonlayın
git clone https://github.com/recepyucegit/BitirmeProjesi.git
cd BitirmeProjesi
```

### Backend Kurulumu
```bash
# API klasörüne gidin
cd src/Presentation/TeknoRoma.API

# NuGet paketlerini yükleyin
dotnet restore

# Projeyi derleyin
dotnet build

# Uygulamayı çalıştırın
dotnet run
```

Backend başarıyla çalıştığında:
- **API**: http://localhost:5000 veya https://localhost:5001
- **Swagger UI**: https://localhost:5001/swagger

### Frontend Kurulumu
```bash
# Frontend klasörüne gidin (yeni terminal)
cd teknoroma-frontend

# npm paketlerini yükleyin
npm install

# Development server'ı başlatın
npm run dev
```

Frontend başarıyla çalıştığında:
- **Web App**: http://localhost:5173

### Veritabanı Yapılandırması

**Not**: Proje şu anda **InMemory Database** kullanıyor. Hiçbir veritabanı kurulumuna gerek yok!

`appsettings.json` dosyasında:
```json
{
  "UseInMemoryDatabase": true
}
```

**SQL Server kullanmak isterseniz**:
1. `appsettings.json` dosyasında `UseInMemoryDatabase: false` yapın
2. `ConnectionStrings:DefaultConnection` bağlantı cümlesini düzenleyin
3. Migration komutlarını çalıştırın:

```bash
cd src/Infrastructure/TeknoRoma.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../../Presentation/TeknoRoma.API
dotnet ef database update --startup-project ../../Presentation/TeknoRoma.API
```

### İlk Kullanıcı (Seed Data)

Uygulama ilk çalıştırıldığında otomatik seed data yüklenir:

**Test Kullanıcıları**:
- **Admin**: `admin` / `admin123`
- **Müdür**: `manager` / `manager123`
- **Kasiyer**: `cashier` / `cashier123`

**Seed Data İçeriği**:
- 6 Kullanıcı (Admin, Manager, Cashier, Warehouse, Accounting, TechService)
- 10 Kategori
- 50 Ürün
- 30 Müşteri
- 20 Tedarikçi
- 30 Çalışan
- 10 Mağaza
- 30 Satış (Shopping Cart ile)
- 20 Tedarikçi Siparişi
- 40 Gider Kaydı

## API Endpoints

### Authentication & User Management
- POST /api/auth/login (JWT Token)
- POST /api/auth/refresh-token (Token Yenileme)
- POST /api/auth/register (Yeni Kullanıcı)
- GET /api/users (Tüm Kullanıcılar)
- GET /api/users/{id} (Kullanıcı Detay)
- POST /api/users (Kullanıcı Oluştur)
- PUT /api/users/{id} (Kullanıcı Güncelle)
- DELETE /api/users/{id} (Kullanıcı Sil - Soft Delete)

### Roles
- GET /api/roles (Tüm Roller)
- GET /api/roles/{id} (Rol Detay)
- POST /api/roles (Rol Oluştur)
- PUT /api/roles/{id} (Rol Güncelle)
- DELETE /api/roles/{id} (Rol Sil)

### Stores
- GET /api/stores (Tüm Mağazalar)
- GET /api/stores/{id} (Mağaza Detay)
- GET /api/stores/city/{city} (Şehir Bazlı)
- GET /api/stores/manager/{managerId} (Müdür Bazlı)
- POST /api/stores (Mağaza Oluştur)
- PUT /api/stores/{id} (Mağaza Güncelle)
- DELETE /api/stores/{id} (Mağaza Sil)

### Expenses
- GET /api/expenses (Tüm Giderler)
- GET /api/expenses/{id} (Gider Detay)
- GET /api/expenses/store/{storeId} (Mağaza Giderleri)
- GET /api/expenses/status/{status} (Durum Bazlı)
- GET /api/expenses/pending (Bekleyen Giderler)
- POST /api/expenses (Gider Oluştur)
- PUT /api/expenses/{id} (Gider Güncelle)
- POST /api/expenses/{id}/approve (Gider Onayla/Reddet)
- DELETE /api/expenses/{id} (Gider Sil)
- GET /api/expenses/total/store/{storeId} (Mağaza Toplam Gider)
- GET /api/expenses/total/category/{category} (Kategori Toplam)

### Categories
- GET /api/categories
- GET /api/categories/{id}
- POST /api/categories
- PUT /api/categories/{id}
- DELETE /api/categories/{id}

### Products
- GET /api/products
- GET /api/products/{id}
- GET /api/products/category/{categoryId}
- GET /api/products/barcode/{barcode}
- POST /api/products
- PUT /api/products/{id}
- DELETE /api/products/{id}

### Sales
- GET /api/sales
- GET /api/sales/{id}
- POST /api/sales
- PUT /api/sales/{id}
- DELETE /api/sales/{id}

### Supplier Transactions
- GET /api/supplier-transactions (Tüm Siparişler - Paginated)
- GET /api/supplier-transactions/{id} (Sipariş Detay)
- GET /api/supplier-transactions/supplier/{supplierId} (Tedarikçi Siparişleri)
- POST /api/supplier-transactions (Sipariş Oluştur)
- PUT /api/supplier-transactions/{id} (Sipariş Güncelle)
- DELETE /api/supplier-transactions/{id} (Sipariş Sil)

### Reports & Dashboard
**Dashboard**
- GET /api/report/dashboard (Dashboard İstatistikleri)

**Sales Reports**
- GET /api/report/sales (Detaylı Satış Raporu - Paginated)
- GET /api/report/sales/summary (Satış Özeti)
- GET /api/report/sales/top-products (En Çok Satan Ürünler)
- GET /api/report/sales/top-customers (En İyi Müşteriler)
- POST /api/report/sales/export (Excel Export)

**Stock Reports**
- GET /api/report/stock (Stok Raporu - Paginated)
- GET /api/report/stock/low-stock (Düşük Stok Uyarıları)
- GET /api/report/stock/summary (Stok Özeti)
- POST /api/report/stock/export (Excel Export)

**Expense Reports**
- GET /api/report/expenses (Gider Raporu - Paginated)
- GET /api/report/expenses/summary (Gider Özeti)
- GET /api/report/expenses/by-category (Kategori Bazlı)
- POST /api/report/expenses/export (Excel Export)

## Güvenlik

- **JWT Token Authentication**: Stateless authentication
- **Refresh Token Mechanism**: Güvenli token yenileme
- **BCrypt Password Hashing**: Güvenli şifre saklama (BCrypt.Net-Next 4.0.3)
- **Role-Based Authorization**: Rol bazlı erişim kontrolü
- **Input Validation**: DTO seviyesinde veri doğrulama
- **SQL Injection Prevention**: EF Core parametreli sorgular
- **XSS Protection**: Input sanitization
- **Soft Delete**: Veri kaybını önleme
- **Audit Trail**: CreatedDate, UpdatedDate, CreatedBy, UpdatedBy tracking

## Test

Proje kapsamlı test coverage'a sahiptir:
- **Unit Tests**: 47 başarılı test
  - User & Role Tests (15 test)
  - Store Tests (8 test)
  - Expense Tests (24 test)
- **Entity Tests**: Tüm entity'lerin doğrulaması
- **Repository Tests**: Repository metodlarının test edilmesi
- **Service Tests**: Business logic testleri

### Test Sonuçları
```bash
cd tests/TeknoRoma.Tests
dotnet test
# Başarılı: 47 | Başarısız: 0 | Atlanan: 0
```

### Test Coverage
- ✓ Entity creation and validation
- ✓ Default values verification
- ✓ Relationships (One-to-Many, Many-to-Many)
- ✓ Business rules validation
- ✓ Approval workflows
- ✓ Multi-currency calculations
- ✓ Status transitions

## Özellikler ve Modüller

### ✅ Tamamlanan Modüller
1. ✅ **Authentication & Authorization** - JWT Token, Role-Based Access
2. ✅ **Category Management** - Kategori CRUD işlemleri
3. ✅ **Product Management** - Ürün yönetimi, stok takibi
4. ✅ **Customer Management** - Müşteri kayıt ve yönetimi
5. ✅ **Supplier Management** - Tedarikçi yönetimi
6. ✅ **Employee Management** - Çalışan kayıtları ve yönetimi
7. ✅ **Sales Management** - Satış işlemleri, shopping cart
8. ✅ **Store Management** - 55 mağaza yönetimi
9. ✅ **Expense Management** - Gider takibi ve onay sistemi
10. ✅ **Supplier Transactions** - Tedarikçi sipariş yönetimi
11. ✅ **Reports Module** - Satış, Stok, Gider raporları
12. ✅ **Dashboard** - Kapsamlı yönetim paneli
13. ✅ **Excel Export** - EPPlus ile Excel dışa aktarma

### 🎨 Frontend Sayfaları
- ✅ Login Page (Giriş)
- ✅ Dashboard / Home Page (Ana Sayfa)
- ✅ Categories (Kategoriler)
- ✅ Products (Ürünler)
- ✅ Customers (Müşteriler)
- ✅ Suppliers (Tedarikçiler)
- ✅ Employees (Çalışanlar)
- ✅ Sales (Satışlar)
- ✅ Stores (Mağazalar)
- ✅ Expenses (Giderler)
- ✅ Supplier Transactions (Tedarikçi Siparişleri)
- ✅ Reports (Raporlar)

## Proje İstatistikleri

**Backend**
- 12 Controller
- 30+ Entity
- 50+ DTO
- 15+ Repository
- 10+ Service
- 100+ API Endpoint

**Frontend**
- 12 Sayfa
- 20+ Komponent
- React Router v6
- Axios HTTP Client
- Bootstrap 5 UI

**Test Coverage**
- 47 Unit Test (Tümü Başarılı)
- Entity Tests
- Repository Tests
- Service Tests

**Seed Data**
- 6 Kullanıcı
- 10 Kategori
- 50 Ürün
- 30 Müşteri
- 20 Tedarikçi
- 30 Çalışan
- 10 Mağaza
- 30 Satış
- 20 Sipariş
- 40 Gider

## Kullanılan Design Patterns

- **Repository Pattern**: Veri erişim katmanı soyutlaması
- **Unit of Work Pattern**: Transaction yönetimi
- **Dependency Injection**: IoC Container (.NET DI)
- **DTO Pattern**: Veri transfer objeleri
- **Factory Pattern**: Bogus seed data üretimi
- **Middleware Pattern**: JWT Authentication
- **Service Layer Pattern**: Business logic ayrımı
- **CQRS (Partial)**: Command/Query ayrımı

## Best Practices

✅ **Clean Architecture** (Onion Architecture)
✅ **SOLID Principles**
✅ **Separation of Concerns**
✅ **DRY (Don't Repeat Yourself)**
✅ **Code First Approach**
✅ **Async/Await Pattern**
✅ **Global Exception Handling**
✅ **Soft Delete Implementation**
✅ **Audit Trail (CreatedBy, UpdatedBy)**
✅ **Pagination Support**
✅ **Input Validation**
✅ **Security Best Practices**

## Lisans

Bu proje **Bitirme Projesi** kapsamında geliştirilmiştir.

**EPPlus NonCommercial License**: Bu projede kullanılan EPPlus kütüphanesi NonCommercial lisans altında kullanılmaktadır.

## İletişim

**GitHub**: [recepyucegit/BitirmeProjesi](https://github.com/recepyucegit/BitirmeProjesi)

Proje ile ilgili sorularınız için issue açabilirsiniz.

---

## Geliştirici Notları

**Not**: Bu proje eğitim amaçlı geliştirilmekte olup, gerçek bir işletme senaryosu (TeknoRoma - 55 mağazalı elektronik perakende zinciri) üzerine kurgulanmıştır.

Modern .NET teknolojileri ve best practices kullanılarak:
- ✅ Clean Architecture prensiplerine uygun
- ✅ SOLID prensipleri ile
- ✅ Test-Driven Development yaklaşımı
- ✅ Industry-standard güvenlik pratikleri
- ✅ Scalable ve maintainable kod yapısı

ile geliştirilmiştir.

**Teknoloji Seçimleri**:
- InMemory Database kullanımı kolay kurulum ve test için idealdir
- React + Vite modern ve hızlı geliştirme deneyimi sağlar
- EPPlus 7.3.2 kararlı ve güvenilir Excel export sunar
- Bootstrap 5 responsive ve modern UI için yeterlidir

**Geliştirme Süreci**:
1. Day 1-2: Authentication & Core Modules
2. Day 3-4: Product & Sales Management
3. Day 4-5: Employee & Sales Features
4. Day 5-6: Store & Expense Modules
5. Day 6-7: Supplier Transactions & Comprehensive Seed Data
6. Day 7-8: Reports Module & Dashboard & Excel Export

**Son Güncelleme**: 2025 - Reports ve Dashboard modülleri tamamlandı
