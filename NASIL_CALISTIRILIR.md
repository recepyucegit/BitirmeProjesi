# TeknoRoma Projesini Çalıştırma Rehberi

Bu rehber, TeknoRoma projesini sunumda canlı gösterim için nasıl çalıştıracağınızı adım adım açıklar.

## 📋 Ön Gereksinimler

Bilgisayarınızda şunlar kurulu olmalı:
- ✅ Visual Studio 2022 (Backend için)
- ✅ Visual Studio Code (Frontend için)
- ✅ Node.js (Frontend için)
- ✅ .NET 7.0 SDK (Backend için)

---

## 🚀 Yöntem 1: VS Code ile Hem Backend Hem Frontend Çalıştırma (ÖNERİLEN)

### Adım 1: VS Code'u Açın
1. VS Code'u açın
2. **File → Open Folder** tıklayın
3. `d:\BitirmeProjesi` klasörünü seçin ve **"Select Folder"** tıklayın

### Adım 2: Backend'i Çalıştırın (Terminal 1)
1. VS Code'da **Terminal → New Terminal** tıklayın (veya `Ctrl + Shift + '`)
2. Terminal'de şu komutları sırayla yazın:

```bash
cd src/Presentation/TeknoRoma.API
dotnet run
```

3. Şu mesajları görmelisiniz:
```
✅ Seed data başarıyla oluşturuldu!
Now listening on: http://localhost:5085
```

4. **Bu terminal'i kapatmayın, açık bırakın!**

### Adım 3: Frontend'i Çalıştırın (Terminal 2)
1. VS Code'da **yeni bir terminal** açın: Sağ üstteki **"+"** butonuna tıklayın
2. Yeni terminal'de şu komutları yazın:

```bash
cd teknoroma-frontend
npm run dev
```

3. Şu mesajı görmelisiniz:
```
VITE v5.x.x ready in 1000ms
➜ Local: http://localhost:5173/
```

4. **Bu terminal'i de kapatmayın!**

### Adım 4: Projeyi Tarayıcıda Açın
1. Chrome veya Edge tarayıcınızı açın
2. Adres çubuğuna yazın: `http://localhost:5173`
3. Login ekranı gelecek
4. Giriş yapın:
   - **Kullanıcı Adı**: `admin`
   - **Şifre**: `admin123`

**🎉 Tebrikler! Proje çalışıyor!**

---

## 🚀 Yöntem 2: Visual Studio ile Backend, VS Code ile Frontend

### Adım 1: Backend'i Visual Studio ile Çalıştırın

1. **Visual Studio 2022**'yi açın
2. **Open a project or solution** tıklayın
3. Şu dosyayı seçin: `d:\BitirmeProjesi\TeknoRoma.sln`
4. Solution açıldıktan sonra:
   - Sağ üstte **"TeknoRoma.API"** seçili olduğundan emin olun
   - Yeşil **▶ Play** butonuna tıklayın (veya `F5`)
   - **VEYA** `Ctrl + F5` (debug olmadan çalıştırmak için)

5. Konsol penceresi açılacak ve şunu göreceksiniz:
```
✅ Seed data başarıyla oluşturuldu!
Now listening on: http://localhost:5085
```

6. **Bu konsol penceresini kapatmayın!**

### Adım 2: Frontend'i VS Code ile Çalıştırın

1. **VS Code**'u açın
2. **File → Open Folder** → `d:\BitirmeProjesi\teknoroma-frontend` seçin
3. **Terminal → New Terminal** (veya `Ctrl + Shift + '`)
4. Terminal'de yazın:

```bash
npm run dev
```

5. Şunu göreceksiniz:
```
VITE v5.x.x ready in 1000ms
➜ Local: http://localhost:5173/
```

### Adım 3: Tarayıcıda Açın
1. Chrome/Edge açın: `http://localhost:5173`
2. Login: `admin` / `admin123`

---

## ⚡ HIZLI BAŞLATMA (Sunum Sırasında)

Sunum başlamadan **5 dakika önce** şunları yapın:

### Hazırlık:
1. ✅ VS Code'u açın ve `d:\BitirmeProjesi` klasörünü yükleyin
2. ✅ 2 terminal hazırlayın (açık bırakın ama çalıştırmayın)
3. ✅ Chrome'da yeni bir tab açın (`http://localhost:5173` yazılı hazır)

### Sunum Başlarken (1 dakika):
```bash
# Terminal 1 (Backend)
cd src/Presentation/TeknoRoma.API && dotnet run

# Terminal 2 (Frontend) - Backend başladıktan 10 saniye sonra
cd teknoroma-frontend && npm run dev
```

### Giriş Bilgileri (Hazır Tutun):
- 👤 **Admin**: `admin` / `admin123`
- 👤 **Manager**: `manager` / `manager123`
- 👤 **Cashier**: `cashier` / `cashier123`

---

## 🛑 Projeyi Durdurmak

### VS Code'da:
- Her iki terminal'de de `Ctrl + C` tuşlarına basın
- VEYA terminal'i kapatın

### Visual Studio'da:
- Kırmızı **■ Stop** butonuna tıklayın
- VEYA konsol penceresini kapatın

---

## ❗ Sık Karşılaşılan Sorunlar ve Çözümleri

### Sorun 1: "Port 5085 already in use"
**Çözüm:**
```bash
# Windows'da portları öldürün
netstat -ano | findstr :5085
taskkill /PID <PID_NUMARASI> /F
```

### Sorun 2: "Port 5173 already in use"
**Çözüm:**
```bash
netstat -ano | findstr :5173
taskkill /PID <PID_NUMARASI> /F
```

### Sorun 3: Frontend başlamıyor - "npm: command not found"
**Çözüm:**
- Node.js kurulu mu kontrol edin: `node --version`
- Kurulu değilse: https://nodejs.org/ indir ve kur

### Sorun 4: "node_modules bulunamadı"
**Çözüm:**
```bash
cd teknoroma-frontend
npm install
```

### Sorun 5: Backend başlamıyor - "dotnet: command not found"
**Çözüm:**
- .NET 7.0 SDK kurulu mu: `dotnet --version`
- Kurulu değilse: https://dotnet.microsoft.com/download

---

## 🎯 Sunum İçin İPUÇLARI

### 1. İki Monitör Kullanıyorsanız:
- **Monitör 1**: VS Code (terminaller görünür)
- **Monitör 2**: Chrome (uygulama görünür)

### 2. Tek Monitör Kullanıyorsanız:
- **Sağ Yarı**: VS Code (küçük tutun, sadece terminaller görünsün)
- **Sol Yarı**: Chrome (büyük tutun, uygulama net görünsün)

### 3. Demo Öncesi Kontrol Listesi:
- ✅ İnternet bağlantısı var mı? (Gerekli değil ama npm install için olabilir)
- ✅ VS Code açık mı?
- ✅ Chrome/Edge açık mı?
- ✅ Giriş bilgileri hazır mı? (Bir yere yazılı)
- ✅ Sunumdan önce bir kez test ettiniz mi?

### 4. Hangi Sayfaları Göstermeli:
1. **Login** → Admin girişi göster
2. **Dashboard** → İstatistikleri göster
3. **Satış** → Yeni satış yap (ürün ekle, sepet, ödeme)
4. **Ürünler** → Filtreleme ve arama göster
5. **Giderler** → Onay/Red işlemi göster (Approval Modal)
6. **Raporlar** → Excel export göster
7. **Kullanıcılar/Roller** → Yönetim paneli göster

### 5. Canlı Demo İçin Senaryo:
```
1. Login yap (admin/admin123)
2. Dashboard'u göster → "Burası anlık istatistikler"
3. Satış yap → Ürün ekle → Ödeme al
4. Giderlere git → Bir gider onayla/reddet
5. Raporlara git → Excel indir
6. Logout yap → Farklı rol ile giriş (manager)
```

---

## 📞 Acil Durum Planı

Eğer canlı demo çalışmazsa:
1. ✅ **Ekran görüntüleri** hazırlayın (her sayfa için)
2. ✅ **Video kaydı** yapın (3-5 dakikalık demo)
3. ✅ **PowerPoint** ile anlat, ekran görüntüleri göster

---

## ✅ Son Kontrol (Sunum Öncesi)

```bash
# Backend Test
curl http://localhost:5085/api/product
# 200 OK dönmeli

# Frontend Test
# Chrome'da http://localhost:5173 açın
# Login ekranı gelmeli
```

---

## 🎓 Sunum Sırasında Anlatılacaklar

1. **Backend Başlatırken**:
   - "Backend'i dotnet run komutuyla başlatıyorum"
   - "Seed data otomatik yüklendi, 32 ürün, 40 gider vb."
   - "API http://localhost:5085 portunda çalışıyor"

2. **Frontend Başlatırken**:
   - "Frontend React + Vite ile geliştirdim"
   - "npm run dev ile başlatıyorum"
   - "http://localhost:5173 portunda çalışıyor"

3. **Giriş Yaparken**:
   - "JWT token ile authentication yapıyoruz"
   - "Admin rolü ile giriş yapıyorum"
   - "Token localStorage'da saklanıyor"

---

## 🎉 BAŞARILAR!

Bu rehberi takip ederseniz, sunumda hiçbir sorun yaşamazsınız!

**Sorularınız varsa tekrar sorun!** 😊
