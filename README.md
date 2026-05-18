# XSLTTransformer

High-performance XML transformation engine for Turkish e-document (e-belge) standards including e-fatura and e-arşiv.  
Yüksek performanslı XML dönüştürme motoru, Türk e-belge standartları (e-fatura, e-arşiv) için tasarlanmıştır.

## Features | Özellikler

- Full XSLT 2.0 and XSLT 3.0 support via Saxon-HE processor  
  Saxon-HE ile tam XSLT 2.0 / 3.0 desteği
- Built-in GİB e-fatura and e-arşiv schema validation library  
  GİB e-fatura ve e-arşiv şema doğrulama kütüphanesi
- High-performance batch processing with parallel execution and real-time progress reporting  
  Paralel çalıştırma ve gerçek zamanlı ilerleme takibi ile toplu işlem
- RESTful API with Swagger/OpenAPI documentation for remote transformations  
  Uzaktan dönüşüm için Swagger belgeli REST API
- Bilingual (TR/EN) WPF desktop application for interactive testing and debugging  
  Etkileşimli test için iki dilli WPF masaüstü uygulaması
- Structured logging and robust error handling for malformed or invalid XML  
  Yapılandırılmış loglama ve hatalı XML için güçlü hata yönetimi
- Compiled stylesheet caching for maximum throughput  
  Maksimum verim için derlenmiş stil sayfası önbellekleme

## Technologies | Teknolojiler

- .NET 8.0 (LTS)
- Saxon-HE 10.9 (XSLT 2/3 engine)
- Serilog (structured logging)
- ASP.NET Core minimal APIs + Swagger
- WPF + MVVM-ready structure
- xUnit for automated testing

## Architecture | Mimari

The solution follows clean architecture principles:

- **XSLTTransformer.Core** — Core engine, schema validator, batch processor, localization and logging facade. Reusable across all layers.
- **XSLTTransformer.Api** — REST API exposing transformation endpoints. Ready for containerization and cloud deployment.
- **XSLTTransformer.Wpf** — Desktop testing tool with full Turkish/English UI. Useful for rapid validation during development.
- **XSLTTransformer.Tests** — Unit and integration tests covering engine, validator and batch scenarios.

All components share the same senior-grade coding standards, nullable reference types and consistent naming.

## How to Build & Run | Derleme ve Çalıştırma

```bash
# Clone repository
git clone https://github.com/omertarikbayir/XSLTTransformer.git
cd XSLTTransformer

# Restore and build entire solution
dotnet restore
dotnet build -c Release

# Run API
dotnet run --project XSLTTransformer.Api --launch-profile https

# Run WPF application
dotnet run --project XSLTTransformer.Wpf
```

## Running Tests | Testleri Çalıştırma

```bash
dotnet test -c Release
```

## Usage Examples | Kullanım Örnekleri

### Simple Transformation (C#)

```csharp
var engine = new XsltEngine();
string result = engine.Transform(xmlContent, xsltContent);
```

### Batch Processing with Progress

```csharp
var processor = new BatchProcessor(engine);
var progress = new Progress<int>(p => Console.WriteLine($"{p}%"));
var results = await processor.ProcessAsync(items, progress: progress);
```

### REST API Call

```http
POST /transform
Content-Type: application/json

{
  "xml": "<Invoice>...</Invoice>",
  "xslt": "<xsl:stylesheet>...</xsl:stylesheet>"
}
```

## Localization | Yerelleştirme

The application fully supports Turkish and English at runtime.  
Uygulama çalışma zamanında Türkçe ve İngilizce tam destek sunar.

## License | Lisans

MIT License – see [LICENSE](LICENSE) file for details.

## Contributing | Katkı

Pull requests and issues are welcome. Please follow the existing code style and add tests for new functionality.

---

**Ready for production use in e-document pipelines.**  
**E-belge işlem hatlarında üretim kullanımı için hazır.**