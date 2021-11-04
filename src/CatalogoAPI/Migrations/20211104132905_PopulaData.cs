using Microsoft.EntityFrameworkCore.Migrations;

namespace CatalogoAPI.Migrations
{
    public partial class PopulaData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO DBCatalogo.Categorias (Nome, ImagemUrl) " +
                                 "VALUES('Celulares', 'https://a-static.mlcdn.com.br/360x270/smartphone-xiaomi-redmi-9a-32gb-cinza-4g-octa-core-2gb-ram-653-cam-13mp-selfie-5mp-dual-chip/magazineluiza/226913400/88c3dcfd9320204adbd890b07c9e9f88.jpg')");

            migrationBuilder.Sql("INSERT INTO DBCatalogo.Categorias (Nome, ImagemUrl) " +
                                "VALUES('Tv e Vídeo', 'https://a-static.mlcdn.com.br/360x270/smart-tv-55-4k-uhd-nanocell-lg-55nano80-60hz-wi-fi-e-bluetooth-alexa-4-hdmi-2-usb/magazineluiza/228863000/edb62dffd62067db6745ebc344fde02d.jpg')");

            migrationBuilder.Sql("INSERT INTO DBCatalogo.Categorias (Nome, ImagemUrl) " +
                                "VALUES('Móveis', 'https://a-static.mlcdn.com.br/360x270/guarda-roupa-casal-com-espelho-3-portas-de-correr-lara-espresso-moveis-branco/madeiramadeira-openapi/174028/77437ebb1d10317feb9f6ee70dd5089e.jpg')");

            migrationBuilder.Sql("INSERT INTO DBCatalogo.Categorias (Nome, ImagemUrl) " +
                                "VALUES('Saldão', 'https://scontent.fplu3-1.fna.fbcdn.net/v/t1.6435-9/119381360_118983739944307_2496626679284785985_n.jpg?_nc_cat=104&ccb=1-5&_nc_sid=e3f864&_nc_ohc=JcwLsIiIum4AX9_RhTt&_nc_ht=scontent.fplu3-1.fna&oh=e77796d78ac520199917c032485f411c&oe=61A8DBDE')");


            migrationBuilder.Sql("INSERT INTO DBCatalogo.Produtos (Nome, Descricao, Valor, ImagemUrl, Estoque, DataCadastro, CategoriaId)" +
                                  "VALUES('Motorola Moto E7', 'Smartphone Motorola Moto E7 32GB', 779.00," +
                                  "'https://a-static.mlcdn.com.br/618x463/smartphone-motorola-moto-e7-32gb-cinza-metalico-4g-octa-core-2gb-ram-65-cam-dupla-selfie-5mp/magazineluiza/155616400/49a5043193c9a8fdaaa3b67d7d0ba1cd.jpg', " +
                                  "7, now(), (SELECT CategoriaId FROM DBCatalogo.Categorias where nome = 'Celulares'))");

            migrationBuilder.Sql("INSERT INTO DBCatalogo.Produtos (Nome, Descricao, Valor, ImagemUrl, Estoque, DataCadastro, CategoriaId)" +
                                  "VALUES('Motorola Moto G20', 'Smartphone Motorola Moto G20 64GB Azul 4G - 4GB RAM Tela 6,5” Câm. Quádrupla + Selfie 13MP', 1.180, " +
                                  "'https://a-static.mlcdn.com.br/618x463/smartphone-motorola-moto-g20-64gb-azul-4g-4gb-ram-tela-65-cam-quadrupla-selfie-13mp/magazineluiza/155641000/b75e9bf34b14f52b75f8437455969784.jpg', " +
                                  "40, now(), (SELECT CategoriaId FROM DBCatalogo.Categorias where nome = 'Celulares'))");

            migrationBuilder.Sql("INSERT INTO DBCatalogo.Produtos (Nome, Descricao, Valor, ImagemUrl, Estoque, DataCadastro, CategoriaId)" +
                                  "VALUES('Smartphone Nokia 5.4', 'Smartphone Nokia 5.4 128GB Azul 4G 4GB RAM - Octa-Core Tela 6,39” Câm. Quádrupla + Selfie 16MP', 1.766, " +
                                  "'https://a-static.mlcdn.com.br/618x463/smartphone-nokia-5-4-128gb-azul-4g-4gb-ram-octa-core-tela-639-cam-quadrupla-selfie-16mp/magazineluiza/232358800/8daa1f9a5b9bf61f6dc01b1dd795f55f.jpg', " +
                                  "10, now(), (SELECT CategoriaId FROM DBCatalogo.Categorias where nome = 'Celulares'))");

            migrationBuilder.Sql("INSERT INTO DBCatalogo.Produtos (Nome, Descricao, Valor, ImagemUrl, Estoque, DataCadastro, CategoriaId)" +
                                  "VALUES('Smart TV 50”', 'Smart TV 50” UHD 4K LED TCL 50P615 VA 60Hz - Android Wi-Fi Bluetooth HDR 3 HDMI 2 USB', 2.672, " +
                                  "'https://a-static.mlcdn.com.br/618x463/smart-tv-50-uhd-4k-led-tcl-50p615-va-60hz-android-wi-fi-bluetooth-hdr-3-hdmi-2-usb/magazineluiza/193445100/b5038d26aeb8d1024b20d5f55288c047.jpg', " +
                                  "30, now(), (SELECT CategoriaId FROM DBCatalogo.Categorias where nome = 'Tv e Vídeo'))");

            migrationBuilder.Sql("INSERT INTO DBCatalogo.Produtos (Nome, Descricao, Valor, ImagemUrl, Estoque, DataCadastro, CategoriaId)" +
                                  "VALUES('Smart TV 60” Crystal 4K Samsung', 'Smart TV 60” Crystal 4K Samsung 60AU8000 Wi-Fi - Bluetooth HDR Alexa Built in 3 HDMI 2 USB', 3.900, " +
                                  "'https://a-static.mlcdn.com.br/618x463/smart-tv-60-crystal-4k-samsung-60au8000-wi-fi-bluetooth-hdr-alexa-built-in-3-hdmi-2-usb/magazineluiza/193442000/38f4877e2f2a77b116c2e0d17aeef950.jpg'," +
                                  " 100, now(), (SELECT CategoriaId FROM DBCatalogo.Categorias where nome = 'Tv e Vídeo'))");

            migrationBuilder.Sql("INSERT INTO DBCatalogo.Produtos (Nome, Descricao, Valor, ImagemUrl, Estoque, DataCadastro, CategoriaId)" +
                                  "VALUES('Sofá Retrátil Reclinável 3', 'Sofá Retrátil Reclinável 3 Lugares Suede - Moscow BestHouse', 902.48, " +
                                  "'https://a-static.mlcdn.com.br/618x463/sofa-retratil-reclinavel-3-lugares-suede-moscow-besthouse/magazineluiza/121936206/cf8ac85a16575d847f2d632780de751a.jpg'," +
                                  " 34, now(), (SELECT CategoriaId FROM DBCatalogo.Categorias where nome = 'Móveis'))");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM DBCatalogo.Produtos");
            migrationBuilder.Sql("DELETE FROM DBCatalogo.Categorias");
        }
    }
}
