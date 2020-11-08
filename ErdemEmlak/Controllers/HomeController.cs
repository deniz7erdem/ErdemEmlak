using ErdemEmlak.Models;
using ErdemEmlak.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
namespace ErdemEmlak.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        db01Entities1 db = new db01Entities1();
        public ActionResult Ilanlar()
        {
            List<ilan> ilan = db.ilans.ToList();
            List<Sehir> sehir = db.Sehirs.ToList();
            
            return View(ilan); ; ;
        }
        public ActionResult Sehir()
        {
            List<Sehir> sehir = db.Sehirs.ToList();
            return PartialView(sehir);
        }

        public ActionResult IlanSehir(int? id)
        {
            Sehir kat = db.Sehirs.Where(s => s.sehirId == id).SingleOrDefault();
            if (kat == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.kategori = kat.sehirAdi;
            List<ilan> ilanlar = db.ilans.Where(s => s.ilanId == id).ToList();
            if (ilanlar.Count == 0)
            {
                ViewBag.kayityok = "Henüz Bu Şehirde İlan Yoktur!";
            }
            return View(ilanlar);
        }
        public ActionResult IlanDetay(int ? id)
        {
            ilan ilan =db.ilans.Where(i => i.ilanId == id).SingleOrDefault();
            if (ilan == null)
            {
                return RedirectToAction("Index");
            }
            return View(ilan);
        }
        [HttpPost]
        public ActionResult ilanArama(string deger)
        {
            ViewBag.deger = deger;

            var aranan = db.ilans.Where(s => s.baslik.Contains(deger) || s.aciklama.Contains(deger)).ToList();
            if (aranan.Count > 0)
            {
                ViewBag.kayityok = aranan.Count + " Adet İlan Bulundu";
            }
            else
            {
                ViewBag.kayityok = "Aradığınız Kritere Uygun İlan Bulunamadı!";
            }


            return View(aranan.OrderByDescending(o => o.ilanId).ToList());

        }

        public ActionResult OturumAc(string returnUrl)
        {

            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult OturumAc(uyeModel model, string returnUrl)
        {
            
            Uye uye = db.Uyes.Where(m => m.kullaniciAdi == model.kullaniciAdi && m.pw == model.pw).SingleOrDefault();
            if (uye != null)
            {
                Session["uyeOturum"] = true;
                Session["uyeId"] = uye.uyeId;
                Session["uyeKadi"] = uye.kullaniciAdi;
                Session["uyeAdmin"] = uye.uyeAdmin;
                if (returnUrl == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return Redirect(returnUrl);
                }

            }
            else
            {
                ViewBag.hata = "Geçersiz Kullanıcı Adı veya Parola!";
                return View();
            }


        }
        public ActionResult OturumKapat(string returnUrl)
        {
            Session.Abandon();
            return Redirect(returnUrl);

        }

        public ActionResult UyeOl()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UyeOl(uyeModel model)
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

            db.Uyes.Add(yeni);
            db.SaveChanges();

            Uye uye = db.Uyes.OrderByDescending(u => u.uyeId).FirstOrDefault();
            Session["uyeOturum"] = true;
            Session["uyeId"] = uye.uyeId;
            Session["uyeKadi"] = uye.kullaniciAdi;
            Session["uyeAdmin"] = uye.uyeAdmin;

            return RedirectToAction("Index");
        }

        public ActionResult Profil(int? id)
        {
            Uye uye = db.Uyes.Where(m => m.uyeId == id).SingleOrDefault();
            if (uye == null)
            {
                return RedirectToAction("Index");
            }
            return View(uye);
        }
        public ActionResult IlanEkle()
        {
            if (Session["uyeOturum"] == null)
            {
                RedirectToAction("OturumAc");
            }
            ilanModel model = getModel();
            System.Diagnostics.Debug.WriteLine(model);

            return View(model);
        }
        [HttpPost]
        public ActionResult IlanEkle(ilanModel model)
        {
            if (Session["uyeOturum"] == null)
            {
                RedirectToAction("OturumAc");
            }
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
            if (Session["uyeOturum"] == null)
            {
                RedirectToAction("OturumAc");
            }
            int uyeId = Convert.ToInt32(Session["uyeId"]);

            ilan ilan = db.ilans.Where(m => m.ilanId == id && m.uyeId == uyeId).SingleOrDefault();
            if (ilan == null)
            {
                return RedirectToAction("UyeDetay/" + uyeId);
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


        public ActionResult IlanSil(int? id)
        {
            if (Session["uyeOturum"] == null)
            {
                RedirectToAction("OturumAc");
            }
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
                return RedirectToAction("Profil/" + uyeId);
            }
            return RedirectToAction("Profil/" + uyeId);
        }

    }
}