using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ErdemEmlak.ViewModel
{
    public class uyeModel
    {
        public int uyeId { get; set; }

        [Required(ErrorMessage = "Kullanıcı Adı Giriniz!")]
        [Display(Name = "Kullanıcı Adı")]
        public string kullaniciAdi { get; set; }


        [Required(ErrorMessage = "E-Posta Giriniz!")]
        [Display(Name = "E-Posta ")]
        public string email { get; set; }


        [Required(ErrorMessage = "Parola Giriniz!")]
        [Display(Name = "Parola")]
        public string pw { get; set; }


        [Required(ErrorMessage = "Adı Soyadı Giriniz!")]
        [Display(Name = "Adı Soyadı")]
        public string Ad_Soyad { get; set; }


        [Required(ErrorMessage = "Telefon Giriniz!")]
        [Display(Name = "Telefon")]
        public string telefon { get; set; }



        [Display(Name = "Üye Statüsü")]
        public int uyeAdmin { get; set; }
    }
}