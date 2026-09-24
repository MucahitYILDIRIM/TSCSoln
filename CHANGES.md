# CHANGES — Unit test altyapısı ve testler

## Başlangıç durumu
- Çözümde hiç test projesi / test framework'ü yoktu (test kapsamı %0).
- Projeler .NET Framework 4.6.1, eski (non-SDK) csproj formatı, ASP.NET MVC 5.2.4.
- `TestBL`, `TestDal` ve `TestController` bağımlılıklarını kendi içinde `new` ile oluşturuyordu; `TestDal` ise
  bağlantı cümlesini doğrudan `ConfigurationManager`'dan okuyordu. Bu yüzden veritabanı olmadan test edilemiyorlardı.

## Kararlar

### Test framework: MSTest v2
- .NET Framework + Visual Studio çözümü için en standart seçenek MSTest (VS'in varsayılan test şablonu).
- Yeni proje: `TSCSoln.Tests` (SDK-style csproj, `net461`), `TSCSoln.sln`'e eklendi.
  - `MSTest.TestFramework` / `MSTest.TestAdapter` 2.2.10, `Microsoft.NET.Test.Sdk` 16.11.0
    (net461'i hâlâ destekleyen son sürüm serisi).
  - `Microsoft.AspNet.Mvc` 5.2.4 (WebUI ile aynı sürüm) controller testleri için.
  - `Microsoft.NETFramework.ReferenceAssemblies`: 4.6.1 targeting pack kurulu olmayan makinede de derlenebilmesi için.
- SDK-style format seçildi çünkü PackageReference ile `packages/` klasörüne dokunmadan paket yönetimi sağlar ve
  `dotnet test` / VS Test Explorer ile doğrudan çalışır. Mevcut eski format projelere ProjectReference ile bağlanır.
- Mocking kütüphanesi eklenmedi; iki basit arayüz için elle yazılmış fake'ler (`Fakes/`) yeterli ve bağımlılığı azaltır.

### Küçük refactor'lar (davranış değişmedi)
| Dosya | Değişiklik | Neden davranış aynı |
|---|---|---|
| `TSCSoln.DataAccess/Test/TestDal.cs` | `ITestDal` arayüzü eklendi; `TestDal(string connectionString)` ctor'u eklendi, parametresiz ctor buna zincirlendi. | Parametresiz ctor aynı config anahtarını aynı şekilde okur (anahtar yoksa yine `NullReferenceException`). |
| `TSCSoln.Business/Test/TestBL.cs` | `ITestBL` arayüzü eklendi; `TestBL(ITestDal)` ctor'u eklendi, parametresiz ctor `new TestDal()` ile buna zincirlendi. | Alan tipi `ITestDal` oldu; varsayılan yolda yine `TestDal` kullanılır. |
| `TSCSoln.WebUI/Controllers/TestController.cs` | `Func<ITestBL>` fabrika alan ctor eklendi; parametresiz ctor `() => new TestBL()` kullanır. | MVC'nin varsayılan aktivatörü parametresiz ctor'u kullanmaya devam eder; `TestBL` önceki gibi her `Index` çağrısında (lazy) oluşturulur. |

- Arayüzler, eski csproj'lara yeni `Compile` girdisi eklememek için mevcut dosyaların içine konuldu.

### Yazılan testler (`TSCSoln.Tests`)
- `DataAccess/UtilityTests` — `Utility.DataTableToList<T>`: boş tablo, kolonsuz tablo, eşleme, satır sırası,
  her satır için ayrı nesne, fazladan kolonların yok sayılması, eksik kolonda varsayılan değer, büyük/küçük harf
  duyarlılığı, `DBNull` davranışı.
- `DataAccess/TestDalTests` — hatalı connection string'de ctor'un `ArgumentException` fırlatması; bağlantı
  açılamadığında `PRC_GET_TESTS`'in hatayı yutup boş liste döndürmesi (ağ erişimi olmadan, boş connection string ile).
- `Business/TestBLTests` — DAL'a tek çağrı ve aynı request ile delege etme, cevabın değiştirilmeden dönmesi, null request.
- `WebUI/TestControllerTests` — varsayılan view + model olarak `TestList`, BL'e tek çağrı, boş liste, BL'in lazy oluşturulması.
- `WebUI/HomeControllerTests` — `Index`/`About`/`Contact` view ve `ViewBag.Message` değerleri.
- `Entities` projesi yalnızca otomatik property'ler içerdiği için ayrı test yazılmadı (dolaylı olarak diğer testlerde kullanılıyor).

### Kapsam dışı bırakılanlar / notlar
- `TestDal.PRC_GET_TESTS`'in başarılı yolu (gerçek stored procedure çağrısı) unit test değil entegrasyon testi
  gerektirir; SQL Server olmadan test edilemez.
- `BundleConfig`, `RouteConfig`, `FilterConfig`, `Global.asax` ve Razor view'lar test edilmedi (framework yapılandırması).
- Mevcut koddaki şüpheli davranışlar **düzeltilmedi**, sadece belgelendi:
  - `Utility.GetItem` DB'den gelen `NULL` (`DBNull`) değerlerde `ArgumentException` fırlatıyor
    (`DataTableToList_DbNullValue_ThrowsArgumentException` bu davranışı sabitliyor).
  - `PRC_GET_TESTS` tüm exception'ları sessizce yutuyor ve bağlantıyı hiç kapatmıyor (`con.Close/Dispose` yok).
- Depoda `bin/` ve `obj/` klasörleri commit'lenmiş durumda; yeni test projesi derlenince de bu klasörler oluşacak.
  Bir `.gitignore` eklenmesi önerilir (bu değişiklikte eklenmedi).

### Doğrulama
- Bu ortamda (macOS) `dotnet`, `mono` ve `msbuild` bulunmadığı için testler **derlenemedi ve çalıştırılamadı**.
  Kod elle dikkatlice incelendi. Windows'ta çalıştırmak için:
  - Visual Studio 2017 (15.9+) veya üstü: Test Explorer → Run All, ya da
  - `msbuild TSCSoln.sln /restore` ardından `vstest.console.exe TSCSoln.Tests\bin\Debug\net461\TSCSoln.Tests.dll`
  - veya `dotnet test TSCSoln.Tests\TSCSoln.Tests.csproj` (WebUI web projesi için VS'in Web hedefleri gerekir).
