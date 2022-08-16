using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public static class Messages
    {
        public static string ProductAdded = "Ürün Eklendi";
        public static string ProductNameInvalid = "Ürün ismi Geçersiz";
        public static string MaintenanceTime = "Sistem Şuanda Bakımda Kuzen";
        public static string ProductsListed = "Ürünler Listelendi";
        public static string ProductCountOfCategoryError="Kategorideki Ürün Limiti Aşıldı";
        public static string ProductNameAlreadyExists="Bu Ürün Zaten Mevcut";
        public static string CategoryLimitExceded= "Kategori Limiti Aşıldı";
        public static string AuthorizationDenied ="Yetkiniz Yoktur";
        public static string UserRegistered="Üye Kayıt Basrili";
        public static string AccessTokenCreated="Token Oluşturuldu";
        public static string UserAlreadyExists="Çıkış Tamamlandı";
        public static string SuccessfulLogin="Giriş Başarılı";
        public static string PasswordError="Şifre Hatalı";
        public static string UserNotFound="Kullanıcı Bulunamadı";
    }
}
