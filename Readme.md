ZAMAN PLANI (Gantt Formatı)
Gün	Görev
1. Gün	Model İnceleme + Katmanlı Mimari ve Gereksinim Analizi + Use Case Diyagramı
2. Gün	İş Tanımları + Uygulama Tipi Değerlendirmesi + Proje Yorumları
3. Gün	Kodlanan Kısımların Durumu + GitHub Döküm + Final Dokümantasyon Dosyası

1.GÜN
Kullanacağım veritabanı modelini belirlemeye çalıştım. Ayrıca VS Code tarafında nasıl bir yapı izlemem gerektiğini belirledim. Geliştirmede kullandığım EF modelleri ve dbcontext’te yaptığım değişiklikleri DB  ile senkronize olması için Data Migration kullanacağım.(Add-Migration InitialCreate; Update-Database)
1. MODEL BİLGİLERİ
Sipariş (Orders)
Kolon Adı	Veri Tipi	Açıklama
Id	int (PK)	Otomatik artan kimlik
OrderDate	DateTime	Sipariş tarihi
DeliveryPoint	string	Teslimat noktası
ReceiverName	string	Alıcı adı
ContactPhone	string	Alıcı telefonu
Status	int	Sipariş durumu (0:Bekliyor, 1:Teslim edildi)
 Sipariş Ürünleri (OrderItems)
Kolon Adı	Veri Tipi	Açıklama
Id	int (PK)	Kimlik
OrderId	int (FK)	İlgili siparişin kimliği
ProductName	string	Ürün adı
Quantity	int	Miktar
İlişki: OrderItems → Orders (1-N)
Teslimat (Deliveries)
Kolon Adı	Veri Tipi	Açıklama
Id	int (PK)	Kimlik
OrderId	int (FK)	Teslim edilen sipariş
DeliveryDate	DateTime	Teslim tarihi
PlateNumber	string	Araç plaka numarası
DeliveredBy	string	Teslimatı yapan kişi
İlişki: Deliveries → Orders (1-1) 
Model yapısı AppDbContext ile EF Core kullanılarak Code-First yaklaşımıyla oluşturulmuştur.
2. KATMANLAR (Katmanlı Mimari)
Proje, üç ana uygulama katmanına ayrılmıştır:
•	API Katmanı (CaseStudy_Netlog.API): RESTful servisleri barındırır.
•	Core Katmanı (CaseStudy_Netlog.Core): Dto’lar, Interface’ler, iş mantığı (business rules) bulunur.
•	Data Katmanı (CaseStudy_Netlog.Data): Entity tanımları ve DbContext burada yer alır.
•	SOAP Servis Katmanı (CoreWcfSoapService): B şirketinden A şirketine veri sağlayan CoreWCF tabanlı servis yapısı.
2.GÜN
Bu aşamada modüllerin ne iş yapacağını belirlemeye çalıştım.
3. İŞ TANIMLARI
Modül	Açıklama
Order Yönetimi	Siparişler alınır, doğrulanır, sisteme kaydedilir.
OrderItem Yönetimi	Siparişe bağlı ürün kalemleri takip edilir.
Teslimat Yönetimi	Sipariş teslimatları plaka ve teslim eden kişiyle takip edilir.
SOAP Entegrasyonu	B şirketi günlük siparişleri XML formatında paylaşır.
REST API	A şirketi teslim edilen siparişleri B şirketine iletir.

5. GEREKSİNİM ANALİZİ (Requirements)
Fonksiyonel Gereksinimler
•	Siparişlerin alınması
•	Sipariş ürünlerinin kaydedilmesi
•	Teslimat bilgilerinin tutulması
•	SOAP servis ile sipariş çekilmesi
•	REST API ile teslimatların gönderilmesi
Teknik Gereksinimler
•	.NET 5.0
•	Entity Framework Core
•	CoreWCF (SOAP)
•	RESTful API
•	Katmanlı mimari
•	Async destekli servisler




6. USE CASE DİYAGRAMI
  

7. PROJE TİPİ YORUMU
Proje Adı	Proje Tipi	Açıklama
CaseStudy_Netlog	Katmanlı Web API (REST)	Web API üzerinden iç hizmetler
CoreWcfSoapService	SOAP Servis (CoreWCF)	Dış entegrasyon için veri paylaşımı
3.GÜN
Yapacağım geliştirme ile alakalı kullanacağım teknolojiler aşağıda belirledim.
8. GELİŞTİRME VE ARAYÜZ
•	Geliştirme Dili: C#
•	Framework: .NET 5.0
•	ORM: Entity Framework Core
•	Veritabanı: SQL Server
•	Frontend: İsteğe bağlı, ama default olarak backend API ve servis ağırlıklı bir yapı seçildi.
•	Tüm alanlar zorunlu input olarak tanımlandı (DataAnnotations: [Required]).
•	Validasyonlar: DTO seviyesinde ve model binding aşamasında yapılmaktadır.
•	Asenkron Kodlama: async/await tüm servislerde uygulanmıştır.
9. GITHUB REPOLAR
•	CaseStudy_Netlog
https://github.com/denisgoksel/CaseStudy_Netlog
•	CoreWcfSoapService
https://github.com/denisgoksel/CoreWcfSoapService

10. DEĞERLENDİRME UYUMLULUĞU
Kriter	Uygunluk
Kod sorunsuz çalışıyor	✅
Anlamlı commitler	✅
Loosely coupled yapı	✅
SOLID prensipler	✅
ORM Kullanımı	✅
Code-First yaklaşımı	✅
Repository Pattern	✅
Katmanlı mimari	✅
REST API async	✅
Validasyonlar	✅

11. PROJENİN ÇALIŞTIRILMASI
	öncelikle db bağlantısı doğru olmalıdır. Daha sonra DB migration yapılabilmesi için Console içerisinde update-database komutu çalıştırılıp. Gerekli olan DB(IntegrationCaseStudy) create edilmelidir.
	Sonrasında CoreWsfSoapService derlenip, run edilmelidir. (Burada bu servis sürekli aynı sample data gönderecek. Bir study olduğu için, Veritabanı bağlantısı yapmak istemedim.)
	Sonrasında CaseStudy_Netlog projesi derlenip, run edilmelidir.
	Örnek olması açısından 2 sipariş Orders tablosuna gelmelidir.
	Şimdi, A şirketi teslim bilgisini güncellediğini farz edelim. (Bunu Swagger UI içersinden “/api/Orders/{id}/mark-delivered” ilgili siparişin id bilgisi girilerek status=1’e çekilir.)
	Status 1 olduktan sonra saatte 1 çalışan servis tetiklenip, teslimat bilgisini delirvery tablosuna iletir. 

İYİ ÇALIŞMALAR
DENİZ GÖKSEL
