# Visual Studio ile Proje Çalıştırma Rehberi

Bu rehber, TeknoRoma projesini **Visual Studio 2022/2026 Insider** kullanarak nasıl çalıştıracağınızı gösterir.

---

## 🎯 Yöntem 1: Visual Studio ile Backend + Frontend (İki Instance)

### Adım 1: İlk Visual Studio - Backend'i Çalıştır

1. **Visual Studio 2026 Insider** (veya 2022)'yi açın
2. **Open a project or solution** tıklayın
3. Şu dosyayı seçin: `d:\BitirmeProjesi\TeknoRoma.sln`
4. Solution açıldıktan sonra:
   - Üstte **Startup Project** olarak `TeknoRoma.API` seçili olmalı
   - Yeşil **▶ TeknoRoma.API** butonuna tıklayın
   - VEYA klavyeden `Ctrl + F5` (Debug olmadan çalıştır - daha hızlı)

5. **Konsol penceresi** açılacak:
```
✅ Seed data başarıyla oluşturuldu!
   - 32 Ürün
   - 40 Gider
   - 30 Satış
Now listening on: http://localhost:5085
Application started. Press Ctrl+C to shut down.
```

6. ✅ **Bu konsol penceresini açık bırakın!**

### Adım 2: İkinci Visual Studio - Frontend'i Çalıştır

1. **Yeni bir Visual Studio 2026 Insider** penceresi açın (ikinci instance)
2. **Open → Folder** tıklayın (Solution değil, Folder!)
3. Şu klasörü seçin: `d:\BitirmeProjesi\teknoroma-frontend`
4. Klasör açıldıktan sonra **View → Terminal** (veya `Ctrl + '`)
5. Terminal'de yazın:

```bash
npm run dev
```

6. Şu çıktıyı göreceksiniz:
```
VITE v5.x.x ready in 1000ms
➜ Local: http://localhost:5173/
➜ press h + enter to show help
```

7. ✅ **Bu Visual Studio penceresini de açık bırakın!**

### Adım 3: Tarayıcıda Açın
- Chrome/Edge açın: `http://localhost:5173`
- Login: `admin` / `admin123`

---

## 🎯 Yöntem 2: Visual Studio ile Backend + Harici Terminal ile Frontend

### Adım 1: Visual Studio - Backend

1. Visual Studio 2026 Insider aç
2. `TeknoRoma.sln` aç
3. `Ctrl + F5` ile çalıştır
4. Backend: http://localhost:5085 ✅

### Adım 2: PowerShell/CMD - Frontend

1. **Windows PowerShell** veya **CMD** açın
2. Şu komutları çalıştırın:

```powershell
cd d:\BitirmeProjesi\teknoroma-frontend
npm run dev
```

3. Frontend: http://localhost:5173 ✅

---

## 🎯 Yöntem 3: Visual Studio Solution'a Frontend Eklemek (İleri Seviye)

Bu yöntemle tek Visual Studio'dan her ikisini de çalıştırabilirsiniz.

### Adım 1: Frontend'i Solution'a Ekleyin

1. Visual Studio'da `TeknoRoma.sln` açın
2. Solution Explorer'da **Solution 'TeknoRoma'** sağ tıklayın
3. **Add → Existing Project** tıklayın
4. **Dosya türünü değiştirin**: Sağ altta "Project Files" yerine **"All Files (*.*)"** seçin
5. `d:\BitirmeProjesi\teknoroma-frontend\package.json` dosyasını seçin
6. "Open" tıklayın

### Adım 2: npm Script'lerini Çalıştırma

1. Solution Explorer'da **teknoroma-frontend** altında **package.json** sağ tıklayın
2. **Task Runner Explorer** açılacak
3. **scripts → dev** sağ tıklayın
4. **Run** tıklayın

### Adım 3: Her İkisini Birden Çalıştır

**Seçenek A: Manuel Start**
1. Backend için: `Ctrl + F5`
2. Frontend için: Task Runner Explorer → dev → Run

**Seçenek B: Multiple Startup Projects (Önerilmez)**
- Visual Studio node.js projelerini doğrudan startup project olarak görmez
- Task Runner Explorer kullanmak daha kolay

---

## 🚀 EN KOLAY YÖNTEM (Önerilen)

### Sunum için En Pratik:

```
┌─────────────────────────────────┐
│ Visual Studio 2026 Insider      │
│ TeknoRoma.sln açık             │
│ Ctrl + F5 → Backend çalışıyor  │
│ http://localhost:5085          │
└─────────────────────────────────┘

┌─────────────────────────────────┐
│ PowerShell / CMD (Frontend)     │
│ cd teknoroma-frontend          │
│ npm run dev                    │
│ http://localhost:5173          │
└─────────────────────────────────┘

┌─────────────────────────────────┐
│ Chrome Browser                  │
│ http://localhost:5173          │
│ Login: admin / admin123        │
└─────────────────────────────────┘
```

---

## 🎓 Visual Studio'da Terminal Kullanma

Eğer Visual Studio içinde terminal kullanmak isterseniz:

### Backend Visual Studio'da:
1. **View → Terminal** (veya `Ctrl + '`)
2. Yeni bir terminal tab açın (+ butonu)
3. Frontend klasörüne git:
```bash
cd d:\BitirmeProjesi\teknoroma-frontend
npm run dev
```

Bu şekilde **tek Visual Studio** penceresinde **2 terminal** ile her ikisini de çalıştırabilirsiniz!

---

## 🎬 Sunum İçin Adım Adım (Visual Studio Yöntemi)

### Hazırlık (5 dakika önce):
1. ✅ Visual Studio 2026 Insider açık, `TeknoRoma.sln` yüklü
2. ✅ PowerShell/CMD hazır (henüz çalıştırma)
3. ✅ Chrome/Edge açık (boş tab)

### Sunum Başlarken (30 saniye):
1. **Visual Studio'da**: `Ctrl + F5` (Backend başlat) → 10 saniye bekle
2. **PowerShell'de**: `npm run dev` (Frontend başlat) → 3 saniye bekle
3. **Chrome'da**: `http://localhost:5173` → Login

---

## 🛑 Projeyi Durdurmak

### Backend (Visual Studio):
- Konsol penceresinde `Ctrl + C`
- VEYA konsol penceresini kapat
- VEYA Visual Studio menüsünde **Debug → Stop Debugging** (`Shift + F5`)

### Frontend (PowerShell/CMD):
- `Ctrl + C` tuşuna bas
- VEYA terminal penceresini kapat

---

## ⚙️ Visual Studio Ayarları ve İpuçları

### 1. Debug vs Release Mode
- **Sunum için**: `Ctrl + F5` (Release mode, daha hızlı)
- **Geliştirme için**: `F5` (Debug mode, breakpoint kullanılabilir)

### 2. Output Penceresini Göster
- **View → Output** (veya `Ctrl + Alt + O`)
- Backend loglarını buradan görebilirsiniz

### 3. Konsol Penceresini Büyütmek
- Konsol penceresi küçükse, kenarından sürükleyerek büyütün
- Böylece seed data mesajlarını daha iyi görebilirsiniz

### 4. Multiple Terminals in Visual Studio
1. **View → Terminal**
2. Terminal panelinde **"+" butonu** → Yeni terminal
3. Terminal 1: Backend çalışıyor
4. Terminal 2: Frontend için `cd teknoroma-frontend && npm run dev`

---

## 🐛 Sık Karşılaşılan Sorunlar

### Sorun 1: "Port 5085 already in use"
**Sebep**: Backend zaten çalışıyor veya önceki oturum kapanmamış

**Çözüm**:
```powershell
# PowerShell'de
netstat -ano | findstr :5085
taskkill /PID <PID_NUMARASI> /F
```

### Sorun 2: "Port 5173 already in use"
**Sebep**: Frontend zaten çalışıyor

**Çözüm**:
```powershell
netstat -ano | findstr :5173
taskkill /PID <PID_NUMARASI> /F
```

### Sorun 3: Visual Studio "TeknoRoma.API" Bulamıyor
**Sebep**: Startup project yanlış seçilmiş

**Çözüm**:
1. Solution Explorer'da **TeknoRoma.API** projesine sağ tıklayın
2. **Set as Startup Project** seçin
3. Proje kalın yazı ile görünecek

### Sorun 4: npm Komutu Çalışmıyor
**Sebep**: Node.js yüklü değil veya PATH'e eklenmemiş

**Çözüm**:
```powershell
# Kontrol et
node --version
npm --version

# Yüklü değilse: https://nodejs.org/ indir ve kur
```

### Sorun 5: "Solution Failed to Load"
**Sebep**: .NET SDK yüklü değil

**Çözüm**:
- Visual Studio Installer açın
- ".NET desktop development" workload'u yükleyin
- VEYA .NET 7.0 SDK indirin: https://dotnet.microsoft.com/download

---

## 📊 Visual Studio Performans İpuçları

### Hızlı Başlatma:
1. **Ctrl + F5** kullanın (`F5` yerine) → Debug olmadan başlar, daha hızlı
2. **Output** penceresini kapatın → Daha az kaynak kullanır
3. **IntelliSense** beklerken başlatmayın → Solution tam yüklensin

### Sunum Sırasında:
1. **Solution Explorer'ı gizleyin** → Ekranda daha fazla yer
2. **Sadece konsol ve tarayıcıyı gösterin** → Sade ve anlaşılır
3. **İki monitör varsa**:
   - Monitör 1: Visual Studio (konsol logları)
   - Monitör 2: Chrome (uygulama demosu)

---

## ✅ Sunum Öncesi Kontrol Listesi

### Visual Studio Kontrolü:
- [ ] Visual Studio 2026 Insider yüklü mü?
- [ ] TeknoRoma.sln açılıyor mu?
- [ ] TeknoRoma.API startup project olarak seçili mi?
- [ ] `Ctrl + F5` ile backend başlıyor mu?

### Frontend Kontrolü:
- [ ] Node.js kurulu mu? (`node --version`)
- [ ] npm kurulu mu? (`npm --version`)
- [ ] `npm install` yapıldı mı? (teknoroma-frontend klasöründe)
- [ ] `npm run dev` çalışıyor mu?

### Tarayıcı Kontrolü:
- [ ] Chrome/Edge yüklü mü?
- [ ] http://localhost:5173 açılıyor mu?
- [ ] Login çalışıyor mu? (admin/admin123)

---

## 🎯 Özet: En İyi Yöntem

**Sunum için en pratik yöntem:**

```
1. Visual Studio 2026 Insider → TeknoRoma.sln aç → Ctrl + F5
   ✅ Backend: http://localhost:5085

2. PowerShell/CMD → cd teknoroma-frontend → npm run dev
   ✅ Frontend: http://localhost:5173

3. Chrome → http://localhost:5173 → Login (admin/admin123)
   ✅ Demo hazır!
```

**Toplam süre: 15-20 saniye** 🚀

---

## 🎉 Başarılar!

Bu rehberi takip ederek Visual Studio ile projeyi kolayca çalıştırabilirsiniz.

**Sorularınız varsa tekrar sorun!** 😊
