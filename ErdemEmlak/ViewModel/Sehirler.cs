using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Web;

namespace ErdemEmlak.ViewModel
{
    public class Sehirler
    {
        [Display(Name = "Şehir Id")]
        public int sehirId { get; set; }

        [Required(ErrorMessage = "Şehir Adı Giriniz")]
        [Display(Name = "Şehir Adı")]
        [StringLength(50, ErrorMessage = " Şehir Adı En Fazla 50 karakter olmalı")]
        public string sehirAdi { get; set; }
    }
}