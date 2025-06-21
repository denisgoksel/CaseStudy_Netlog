
 PROJENİN ÇALIŞTIRILMASI
-	öncelikle db bağlantısı doğru olmalıdır. Daha sonra DB migration yapılabilmesi için Console içerisinde update-database komutu çalıştırılıp. Gerekli olan DB(IntegrationCaseStudy) create edilmelidir.
-	Sonrasında CoreWsfSoapService derlenip, run edilmelidir. (Burada bu servis sürekli aynı sample data gönderecek. Bir study olduğu için, Veritabanı bağlantısı yapmak istemedim.)
-	Sonrasında CaseStudy_Netlog projesi derlenip, run edilmelidir.
-	Örnek olması açısından 2 sipariş Orders tablosuna gelmelidir.
-	Şimdi, A şirketi teslim bilgisini güncellediğini farz edelim. (Bunu Swagger UI içersinden “/api/Orders/{id}/mark-delivered” ilgili siparişin id bilgisi girilerek status=1’e çekilir.)
-	Status 1 olduktan sonra saatte bir çalışan servis tetiklenip, teslimat bilgisini delirvery tablosuna iletir. Status durumunu 2'ye çeker.'

İYİ ÇALIŞMALAR
DENİZ GÖKSEL
