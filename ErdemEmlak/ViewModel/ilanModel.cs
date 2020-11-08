using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;
using System.Web;

namespace ErdemEmlak.ViewModel
{
    public class ilanModel
    {
        public int ilanId { get; set; }

        [Required(ErrorMessage = "Başlık Giriniz!")]
        [Display(Name = "İlan Başlığı")]
        public string baslik { get; set; }

        [Required(ErrorMessage = "Fiyat Giriniz!")]
        [Display(Name = "Fiyat")]
        public int? fiyat { get; set; }

        [Required(ErrorMessage = "Adres Giriniz!")]
        [Display(Name = "Adres")]
        public string adres { get; set; }

        [Required(ErrorMessage = "Açıklama Giriniz!")]
        [Display(Name = "Açıklama")]
        public string aciklama { get; set; }

        [Required(ErrorMessage = "Şehir Seçiniz!")]
        [Display(Name = "Şehir")]
        public int? sehirId { get; set; }
        public List<SelectListItem> sehirList { get; set; }

        [Required(ErrorMessage = "Fotoğraf Seçiniz!")]
        [Display(Name = "Fotoğraf")]
        public HttpPostedFileBase foto { get; set; }

        public int uyeId { get; set; }

    }
}