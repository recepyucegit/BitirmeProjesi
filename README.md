# TeknoRoma - Elektronik Mağaza Yönetim Sistemi

## Proje Hakkında

TeknoRoma, 55 mağaza ve 258 çalışan ile Türkiye'nin önde gelen elektronik perakende zincirlerinden biri için geliştirilmiş modern bir yönetim sistemidir. Proje, tedarikçi yönetimi, stok takibi, satış işlemleri ve kapsamlı raporlama özelliklerini içerir.

## Teknoloji Stacki

- **Backend**: ASP.NET Core 7.0 Web API
- **Mimari**: Onion Architecture (Clean Architecture)
- **Database**: SQL Server
- **ORM**: Entity Framework Core
- **Frontend**: React (Modern Web Application)

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

### 7. Depo Yönetimi
- Gerçek zamanlı stok takibi
- Satış-depo entegrasyonu
- Ürün hazırlama ve teslimat yönetimi

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

### Şube Müdürü Raporları

1. **Stok Takip Raporu**
   - Ürün stok durumları
   - Kritik seviyenin altındaki ürünler
   - Kategori bazlı stok analizi

2. **Satış Takip Raporu**
   - Çalışan bazlı satış performansı
   - Satış kotası ve prim hesaplaması
   - En çok satan 10 ürün
   - Müşteri kitle analizi (yaş, cinsiyet)
   - Cross-selling analizi (birlikte satılan ürünler)

3. **Tedarikçi Hareket Raporu**
   - Aylık alım detayları
   - Tedarikçi bazlı harcamalar
   - Ürün bazlı tedarik analizi

4. **Ürün Liste Raporu**
   - Kategori bazlı ürün listeleme
   - Fiyat ve stok bilgileri
   - Satılmayan ürünler (Slow-moving stock)

5. **Gider Raporu**
   - Tüm gider kategorileri
   - Döviz kuru bazlı hesaplama
   - Aylık/yıllık karşılaştırma

### Rapor Formatları
- Excel Export
- PDF Export
- Ekran Görüntüleme
- Yazdırma

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
- .NET 7.0 SDK
- SQL Server 2019 veya üzeri
- Node.js 18+ (Frontend için)

### Backend Kurulumu
```bash
cd src/Presentation/TeknoRoma.API
dotnet restore
dotnet build
dotnet run
```

### Frontend Kurulumu
```bash
cd teknoroma-frontend
npm install
npm run dev
```

### Database Migration
```bash
cd src/Infrastructure/TeknoRoma.Infrastructure
dotnet ef migrations add InitialCreate
dotnet ef database update
```

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

### Reports
- GET /api/reports/stock
- GET /api/reports/sales
- GET /api/reports/suppliers
- GET /api/reports/expenses

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

## Katkıda Bulunanlar

- Backend Development Team
- Frontend Development Team
- Database Design Team

## Lisans

Bu proje Bitirme Projesi kapsamında geliştirilmiştir.

## İletişim

Proje ile ilgili sorularınız için issue açabilirsiniz.

---

**Not**: Bu proje eğitim amaçlı geliştirilmekte olup, gerçek bir işletme senaryosu üzerine kurgulanmıştır. Modern .NET teknolojileri ve best practices kullanılarak Clean Architecture prensiplerine uygun şekilde geliştirilmiştir.
