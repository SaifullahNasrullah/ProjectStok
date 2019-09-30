using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectStok.Models.Entity;
using PagedList;
using PagedList.Mvc;
namespace ProjectStok.Controllers
{
    public class YoneticiController : Controller
    {
        // GET: Yonetici
        StokEntities db = new StokEntities(); // Database bağlantısı
        #region KATEGORİ İŞLEMLERİ
        public ActionResult Index(int sayfa=1)
        {
            //var veriler = db.TBLKategoriler.ToList();
            var veriler = db.TBLKategoriler.ToList().ToPagedList(sayfa, 4);//Sayfalama için, sayfa değişkeni page başlangici için kullanıldı
            return View(veriler);
        }
        [HttpGet]//Sadece Sayfaya yönlendirecek. Button'a basmadan işlem yapılmayacak.
        public ActionResult YeniKategoriEkle()
        {
            return View();
        }
        [HttpPost]//Button'a bastıktan sonra ekleme işlemi gerçekleşecek.
        public ActionResult YeniKategoriEkle(TBLKategoriler yeniKategori)
        {
            if (!ModelState.IsValid)
            {
                return View("YeniKategoriEkle");
            }
            else
            {
                _ = db.TBLKategoriler.Add(yeniKategori);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult kategoriSil(int id)
        {
            var silenecekKategori = db.TBLKategoriler.Find(id);
            db.TBLKategoriler.Remove(silenecekKategori);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult KategoriGuncelle(int id)
        {
            var kategori = db.TBLKategoriler.Find(id);
            
            return View("KategoriGuncelle",kategori);
        }
        public ActionResult KategoriGuncelleKaydet(TBLKategoriler kategori)
        {
            var ktgr = db.TBLKategoriler.Find(kategori.KategoriID); 
            ktgr.KategoriAd = kategori.KategoriAd;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion


        #region ÜRÜN İŞLEMLERİ
        public ActionResult UrunlerListele(int sayfa=1)
        {
            //var urunler = db.TBLUrunler.ToList();
            var urunler = db.TBLUrunler.ToList().ToPagedList(sayfa, 4);
            return View(urunler);
        }
         
        [HttpGet]
        public ActionResult YeniUrunEkle()
        {
            //ViewBag.Kategori = new SelectList(db.TBLKategoriler, "KategoriID", "KategoriAd");
            List<SelectListItem> liste = (from kt in db.TBLKategoriler.ToList()
                                          select new SelectListItem
                                          {
                                              Text = kt.KategoriAd,
                                              Value = kt.KategoriID.ToString()

                                          }).ToList();
            //ViewBag.kategoriList = liste;
            ViewData["KategoriList"] = liste;
            return View();
        }
        //Buradan devam edceğiz...
        [HttpPost]
        public ActionResult YeniUrunEkle(TBLUrunler yeniUrun)
        {
            var kategori = db.TBLKategoriler.Where(m => m.KategoriID == yeniUrun.TBLKategoriler.KategoriID).FirstOrDefault();
            yeniUrun.TBLKategoriler = kategori; 
            db.TBLUrunler.Add(yeniUrun);
            _ = db.SaveChanges(); 
            return RedirectToAction("UrunlerListele");
        }
        public ActionResult UrunSil(int id)
        {
            var silinecekUrun = db.TBLUrunler.Find(id);
            db.TBLUrunler.Remove(silinecekUrun);
            db.SaveChanges();
            return RedirectToAction("UrunlerListele");
        }
        public ActionResult UrunGuncelle(int id)
        {
            var urun = db.TBLUrunler.Find(id);

            List<SelectListItem> liste = (from kt in db.TBLKategoriler.ToList()
                                          select new SelectListItem
                                          {
                                              Text = kt.KategoriAd,
                                              Value = kt.KategoriID.ToString()

                                          }).ToList();
            //ViewBag.kategoriList = liste;
            ViewData["KategoriList"] = liste;

            return View("UrunGuncelle", urun);
        }
        public ActionResult UrunGuncellemeyiKaydet(TBLUrunler urun)
        {
            var urunler = db.TBLUrunler.Find(urun.UrunID); 
            urunler.UrunAdi = urun.UrunAdi; 
            urunler.Marka = urun.Marka;
            urunler.Fiyat = urun.Fiyat;
            urunler.Stok = urun.Stok;
            var kategori = db.TBLKategoriler.Where(u => u.KategoriID == urun.TBLKategoriler.KategoriID).FirstOrDefault();
            urunler.UrunKategori = kategori.KategoriID;
            _ = db.SaveChanges();
            return RedirectToAction("UrunlerListele");
        }
        #endregion

        #region MÜŞTERİ İŞLEMLERİ
        public ActionResult MusteriListele(int sayfa=1)
        {
            //var musteri = db.TBLMusteriler.ToList();
            var musteri = db.TBLMusteriler.ToList().ToPagedList(sayfa, 4);
            return View(musteri); 
        }
        [HttpGet]
        public ActionResult YeniMusteriEkle()
        {
            return View();
        }
        public ActionResult YeniMusteriEkle(TBLMusteriler yeniMushteri)
        {
            if (!ModelState.IsValid)
            {
                return View("YeniMusteriEkle");
            }
            else { 
                _ = db.TBLMusteriler.Add(yeniMushteri);
                db.SaveChanges();
            }
            return RedirectToAction("MusteriListele");
        }
        public ActionResult MusteriSil(int id)
        {
            var Silenecek = db.TBLMusteriler.Find(id);
            db.TBLMusteriler.Remove(Silenecek);
            db.SaveChanges();
            return RedirectToAction("MusteriListele");
        }
        public ActionResult MusteriGuncelle(int id)
        {
            var musteri = db.TBLMusteriler.Find(id);
            return View("MusteriGuncelle", musteri);
        }
        public ActionResult MusteriGuncellemeyiKaydet(TBLMusteriler musteri)
        {
            var mstr = db.TBLMusteriler.Find(musteri.MusterID); 
            mstr.MusterAd = musteri.MusterAd;
            mstr.MusteriSoyad = musteri.MusteriSoyad;
            db.SaveChanges();
            return RedirectToAction("MusteriListele");
        }
        #endregion

        #region SATIŞ İŞLEMLERİ
        public ActionResult SatisListele()
        { 
            return View();
        }
        [HttpGet]
        public ActionResult YeniSatis()
        {
            return View();
        }
        [HttpPost]
        public ActionResult YeniSatis(TBLSatis p)
        {
            db.TBLSatis.Add(p);
            db.SaveChanges();
            return View("SatisListele");
        }
        #endregion
        #region YARDIM SAYFASI
        public ActionResult Yardim()
        {
            return View();
        }
        #endregion
    }
}