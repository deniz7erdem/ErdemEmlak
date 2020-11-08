using ErdemEmlak.Models;
using ErdemEmlak.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ErdemEmlak.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        db01Entities1 db = new db01Entities1();

        public ActionResult Index()
        {
            List<Sehir> sehirler = db.Sehirs.ToList();
            ViewBag.sehirSay = sehirler.Count;

            List<ilan> ilanlar = db.ilans.ToList();
            ViewBag.ilanSay = ilanlar.Count;

            List<Uye> uyeler = db.Uyes.ToList();
            ViewBag.uyeSay = uyeler.Count;
            return View();
        }
        public ActionResult Sehirler(int? id)
        {
            if (id == 1)
            {
                ViewBag.hata = "Şehire Ait İlan Olduğu İçin Kategori Silinemez!";
            }
            if (id == 2)
            {
                ViewBag.sonuc = "Şehir Silindi";
            }
            List<Sehir> kategoriler = db.Sehirs.ToList();
            return View(kategoriler);
        }
        public ActionResult SehirEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SehirEkle(Sehirler model)
        {
            if (db.Sehirs.Where(m => m.sehirAdi == model.sehirAdi).Count() > 0)
            {
                ViewBag.hata = "Girilen Kategori Kayıtlıdır!";
                return View();
            }
            else
            {
                Sehir seh = new Sehir();
                seh.sehirAdi = model.sehirAdi;

                db.Sehirs.Add(seh);
                db.SaveChanges();
                ViewBag.sonuc = "Kategori Eklendi";

                return View();
            }

        }

        public ActionResult SehirDuzenle(int id)
        {
            Sehir seh = db.Sehirs.Where(m => m.sehirId == id).SingleOrDefault();
            Sehirler model = new Sehirler();
            model.sehirId = seh.sehirId;
            model.sehirAdi = seh.sehirAdi;
            return View(model);
        }
        [HttpPost]
        public ActionResult SehirDuzenle(Sehirler model)
        {
            Sehir seh = db.Sehirs.Where(m => m.sehirId == model.sehirId).SingleOrDefault();
            seh.sehirAdi = model.sehirAdi;

            db.SaveChanges();

            ViewBag.sonuc = "Kayıt Güncellendi";

            return View();
        }
        public ActionResult SehirSil(int? id)
        {
            if (db.Sehirs.Where(m => m.sehirId == id).Count() > 0)
            {
                return RedirectToAction("Sehirler/1");
            }

            Sehir kat = db.Sehirs.Where(m => m.sehirId == id).SingleOrDefault();
            if (kat != null)
            {
                db.Sehirs.Remove(kat);
                db.SaveChanges();
                return RedirectToAction("Sehirler/2");
            }
            return RedirectToAction("Sehirler");
        }

        public ActionResult Ilanlar(int? id)
        {
            if (id == 1)
            {
                ViewBag.sonuc = "İlan Silindi";
            }
            List<ilan> ilan = db.ilans.ToList();
            return View(ilan);
        }
        public ActionResult IlanEkle()
        {
            ilanModel model = getModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult IlanEkle(ilanModel model)
        {
            int uyeId = Convert.ToInt32(Session["uyeId"]);

            if (db.ilans.Where(m => m.baslik == model.baslik).Count() > 0)
            {
                ViewBag.hata = "Girilen İlan Kayıtlıdır!";
                return View();
            }
            else
            {
                ilan ilan = new ilan();
                if (model.foto != null && model.foto.ContentLength > 0)
                {
                    string dosya = Guid.NewGuid().ToString();
                    string uzanti = Path.GetExtension(model.foto.FileName).ToLower();

                    if (uzanti != ".jpg" && uzanti != ".jpeg" && uzanti != ".png")
                    {
                        ModelState.AddModelError("Foto", "Dosya Uzantısı JPG,JPEG veya PNG Olmalıdır!");
                        return View(model);
                    }

                    string dosyaAdi = dosya + uzanti;
                    model.foto.SaveAs(Server.MapPath("~/Content/img/ilan/" + dosyaAdi));

                    ilan.foto = dosyaAdi;

                    ilan.baslik = model.baslik;
                    ilan.fiyat = model.fiyat;
                    ilan.adres = model.adres;
                    ilan.aciklama = model.aciklama;
                    ilan.sehirId = model.sehirId;
                    ilan.aciklama = model.aciklama;
                    ilan.uyeId = uyeId;

                    db.ilans.Add(ilan);
                    db.SaveChanges();
                    ViewBag.sonuc = "İlan Eklendi";

                    model = getModel();
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError("Foto", "Dosya Seçim Hatası!");
                    model = getModel();
                    return View(model);
                }
            }
        }

        public ActionResult IlanDuzenle(int? id)
        {
            int uyeId = Convert.ToInt32(Session["uyeId"]);

            ilan ilan = db.ilans.Where(m => m.ilanId == id && m.uyeId == uyeId).SingleOrDefault();
            if (ilan == null)
            {
                return RedirectToAction("Ilanlar/");
            }
            ilanModel model = getModel();
            model.ilanId = ilan.ilanId;
            model.baslik = ilan.baslik;
            model.fiyat = ilan.fiyat;
            model.adres = ilan.adres;
            model.aciklama = ilan.aciklama;
            model.sehirId = ilan.sehirId;
            return View(model);
        }
        [HttpPost]
        public ActionResult IlanDuzenle(ilanModel model)
        {
            ilan ilan = db.ilans.Where(m => m.ilanId == model.ilanId).SingleOrDefault();
            ilan.baslik = model.baslik;
            ilan.fiyat = model.fiyat;
            ilan.adres = model.adres;
            ilan.aciklama = model.aciklama;
            ilan.sehirId = model.sehirId;

            db.SaveChanges();
            ViewBag.sonuc = "İlan Güncellendi";
            model = getModel();
            return View(model);
        }
        public ActionResult IlanSil(int? id)
        {
            int uyeId = Convert.ToInt32(Session["uyeId"]);

            ilan ilan = db.ilans.Where(m => m.ilanId == id && m.uyeId == uyeId).SingleOrDefault();
            if (ilan != null)
            {
                if (System.IO.File.Exists(Server.MapPath("~/Content/img/ilan/" + ilan.foto)))
                {
                    System.IO.File.Delete(Server.MapPath("~/Content/img/ilan/" + ilan.foto));
                }
                db.ilans.Remove(ilan);
                db.SaveChanges();
                return RedirectToAction("Ilanlar/1");
            }
            return RedirectToAction("Ilanlar/");
        }


        private ilanModel getModel()
        {
            ilanModel model = new ilanModel();
            model.sehirList = (from kat in db.Sehirs.ToList()
                               select new SelectListItem
                               {
                                   Selected = false,
                                   Text = kat.sehirAdi,
                                   Value = kat.sehirId.ToString()
                               }).ToList();
            model.sehirList.Insert(0, new SelectListItem
            {
                Selected = true,
                Value = "",
                Text = "Seçiniz"
            });
            return model;
        }
        public ActionResult Uyeler(int? id)
        {
            if (id == 1)
            {
                ViewBag.hata = "Üyeye Ait İlan Olduğu İçin Üye Silinemez!";
            }
            if (id == 2)
            {
                ViewBag.sonuc = "Üye Silindi";
            }
            List<Uye> uyeler = db.Uyes.ToList();
            return View(uyeler);
        }

        public ActionResult UyeEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UyeEkle(uyeModel model)
        {
            if (db.Uyes.Where(m => m.kullaniciAdi == model.kullaniciAdi).Count() > 0)
            {
                ViewBag.hata = "Girilen Kullanıcı Adı Kayıtlıdır!";
                return View();
            }
            Uye yeni = new Uye();


            yeni.kullaniciAdi = model.kullaniciAdi;
            yeni.pw = model.pw;
            yeni.Ad_Soyad = model.Ad_Soyad;
            yeni.email = model.email;
            yeni.telefon = model.telefon;
            yeni.uyeAdmin = model.uyeAdmin;
            db.Uyes.Add(yeni);
            db.SaveChanges();
            ViewBag.sonuc = "Üye Eklendi";
            return View();
        }

        public ActionResult UyeDuzenle(int? id)
        {
            Uye uye = db.Uyes.Where(m => m.uyeId == id).SingleOrDefault();
            if (uye == null)
            {
                return RedirectToAction("Uyeler");
            }
            uyeModel model = new uyeModel();
            model.uyeId = uye.uyeId;
            model.kullaniciAdi = uye.kullaniciAdi;
            model.pw = uye.pw;
            model.Ad_Soyad = uye.Ad_Soyad;
            model.email = uye.email;
            model.telefon = uye.telefon;
            model.uyeAdmin = uye.uyeAdmin;
            return View(model);
        }
        [HttpPost]
        public ActionResult UyeDuzenle(uyeModel model)
        {
            Uye uye = db.Uyes.Where(m => m.uyeId == model.uyeId).SingleOrDefault();
            uye.kullaniciAdi = model.kullaniciAdi;
            uye.pw = model.pw;
            uye.Ad_Soyad = model.Ad_Soyad;
            uye.email = model.email;
            uye.telefon = model.telefon;
            uye.uyeAdmin = model.uyeAdmin;
            db.SaveChanges();
            ViewBag.sonuc = "Üye Güncellendi";
            return View();
        }
        public ActionResult UyeSil(int? id)
        {
            if (db.ilans.Where(m => m.uyeId == id).Count() > 0)
            {
                return RedirectToAction("Uyeler/1");
            }

            Uye uye = db.Uyes.Where(m => m.uyeId == id).SingleOrDefault();
            if (uye != null)
            {
                
                db.Uyes.Remove(uye);
                db.SaveChanges();
                return RedirectToAction("Uyeler/2");
            }
            return RedirectToAction("Uyeler");
        }










    }
}