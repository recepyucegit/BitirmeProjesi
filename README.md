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

### 1. Kullanıcı Yönetimi
- Rol bazlı yetkilendirme sistemi
- Kullanıcı grupları: Şube Müdürü, Kasa Satış, Depo, Muhasebe, Teknik Servis

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

### 7. Depo Yönetimi
- Gerçek zamanlı stok takibi
- Satış-depo entegrasyonu
- Ürün hazırlama ve teslimat yönetimi

### 8. Muhasebe Modülü
- Gider takibi
  - Çalışan ödemeleri
  - Teknik altyapı giderleri
  - Faturalar
  - Diğer giderler
- Döviz kuru entegrasyonu
- Gelir-gider raporlaması

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
- SupplierTransactions (Tedarikçi Hareketleri)

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

### Authentication
- POST /api/auth/login
- POST /api/auth/register

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

- JWT Token Authentication
- Role-Based Authorization
- Input Validation
- SQL Injection Prevention
- XSS Protection

## Test

Proje kapsamlı test coverage'a sahiptir:
- Unit Tests (28 test)
- Integration Tests
- Repository Tests
- Service Tests

```bash
cd tests/TeknoRoma.Tests
dotnet test
```

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
