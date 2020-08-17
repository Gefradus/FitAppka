using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitAppka.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace NowyDotnecik.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class PosilekController : Controller
    {
        private readonly FitAppContext _context;
        private readonly IWebHostEnvironment _env;

        public PosilekController(FitAppContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> OknoGlowne(DateTime dzienWybrany)
        {
            int klientID = GetLoggedInClientID();

            CheckIfAdmin(klientID);
            DodajNowyDzien(dzienWybrany, klientID);

            ViewData["dzien"] = dzienWybrany;
            ViewData["dzienNapis"] = FormatDaty(dzienWybrany);
            ViewData["datepick"] = dzienWybrany.ToString("yyyy-MM-dd");
            ViewData["path"] = _env.WebRootPath.ToString();
            PrzeslijDaneDoWidoku(klientID, _context.Dzien.Where(d => d.Dzien1 == dzienWybrany && d.KlientId == klientID).Select(d => d.DzienId).FirstOrDefault(), 0, 0);
            
            SprawdzPosilki(dzienWybrany, klientID);
            PoliczKalorie(1,"kcalSniadanie", dzienWybrany, klientID);
            PoliczKalorie(2,"kcalSniadanie2", dzienWybrany, klientID);
            PoliczKalorie(3,"kcalObiad", dzienWybrany, klientID);
            PoliczKalorie(4,"kcalDeser", dzienWybrany, klientID);
            PoliczKalorie(5,"kcalPrzekaska", dzienWybrany, klientID);
            PoliczKalorie(6,"kcalKolacja", dzienWybrany, klientID);
            WyswietlWode(dzienWybrany, klientID);
            PoliczPostep(dzienWybrany, klientID);      

            var fitAppContext = _context.Posilek.Include(p => p.Dzien).Include(p => p.Produkt);
            
            return View(await fitAppContext.ToListAsync());
            
        }

        private void CheckIfAdmin(int klientID)
        {
            Klient klient = _context.Klient.Where(k => k.KlientId == klientID).FirstOrDefault();
            if (klient.CzyAdministrator)
            {
                ViewData["admin"] = 1;
            }
            else
            {
                ViewData["admin"] = 0;
            }
        }

        private int GetLoggedInClientID()
        {
            return _context.Klient.Where(k => k.Login.ToLower().Equals(User.Identity.Name.ToLower())).Select(k => k.KlientId).FirstOrDefault();
        }

        public IActionResult Start()
        {
            if (CzyPierwszeUruchomienie(GetLoggedInClientID()))
            {
                return RedirectToAction("Settings", "Settings", new { czyPierwsze = 1 });
            }
            
            return RedirectToAction(nameof(OknoGlowne), new { dzienWybrany = DateTime.Now.Date});
        }

        private bool CzyPierwszeUruchomienie(int klientID)
        {
            var klient = _context.Klient.Where(k => k.KlientId == klientID);

            return klient.Select(k => k.CelWegl).FirstOrDefault() == null;
        }

        public IActionResult Powrot(int klientID, int dzienID)
        {
            DateTime dzienWybrany;
            var dzien = _context.Dzien.Where(d => d.DzienId == dzienID).Select(d => d.Dzien1);
            if (dzien != null && dzienID != 0 && klientID != 0)
            {
                dzienWybrany = (DateTime)dzien.FirstOrDefault();
            }
            else
            {
                dzienWybrany = DateTime.Now.Date;
                klientID = GetLoggedInClientID();
            }
  
            return RedirectToAction(nameof(OknoGlowne), new { klientID, dzienWybrany});
        }


        public IActionResult WybierzDzien(int id, int dzienID, int klientID)
        {
            
            DateTime dzienWybrany = (DateTime) _context.Dzien.Where(d => d.DzienId == dzienID).Select(d => d.Dzien1).FirstOrDefault();

            if (id == 1)    //poprzedni dzień
            {
                dzienWybrany = dzienWybrany.AddDays(-1);
                return RedirectToAction(nameof(OknoGlowne), new { klientID, dzienWybrany });

            }
            else   //następny dzień
            {
                dzienWybrany = dzienWybrany.AddDays(1);
                return RedirectToAction(nameof(OknoGlowne), new { klientID, dzienWybrany});
            }
 
        }

        private void PrzeslijDaneDoWidoku(int klientID, int dzienID, int wKtorym, int? posilekID)
        {
            ViewData["klientID"] = klientID;
            ViewData["dzienID"] = dzienID;
            ViewData["wKtorym"] = wKtorym;
            ViewData["posilekID"] = posilekID;
        }


        private string FormatDaty(DateTime dzienWybrany)
        {
            int dayOfWeek = (int) dzienWybrany.DayOfWeek;
            string dzienTygodnia = "";
            string miesiac = "";

            if(dayOfWeek == 0){ dzienTygodnia = "Niedziela, "; }
            if(dayOfWeek == 1){ dzienTygodnia = "Poniedziałek, "; }
            if(dayOfWeek == 2){ dzienTygodnia = "Wtorek, "; }
            if(dayOfWeek == 3){ dzienTygodnia = "Środa, "; }
            if(dayOfWeek == 4){ dzienTygodnia = "Czwartek, "; }
            if(dayOfWeek == 5){ dzienTygodnia = "Piątek, "; }   
            if(dayOfWeek == 6){ dzienTygodnia = "Sobota, "; }

            if (dzienWybrany.Month == 1) { miesiac = "sty, "; }
            if (dzienWybrany.Month == 2) { miesiac = "lut, "; }
            if (dzienWybrany.Month == 3) { miesiac = "mar, "; }
            if (dzienWybrany.Month == 4) { miesiac = "kwi, "; }
            if (dzienWybrany.Month == 5) { miesiac = "maj, "; }
            if (dzienWybrany.Month == 6) { miesiac = "czer, "; }
            if (dzienWybrany.Month == 7) { miesiac = "lip, "; }
            if (dzienWybrany.Month == 8) { miesiac = "sie, "; }
            if (dzienWybrany.Month == 9) { miesiac = "wrz, "; }
            if (dzienWybrany.Month == 10) { miesiac = "paź, "; }
            if (dzienWybrany.Month == 11) { miesiac = "lis, "; }
            if (dzienWybrany.Month == 12) { miesiac = "gru, "; }
            
            if (dzienWybrany == DateTime.Now.Date)
            {
                dzienTygodnia = "Dzisiaj, ";
            }

            if (dzienWybrany == DateTime.Now.Date.AddDays(-1))
            {
                dzienTygodnia = "Wczoraj, ";
            }

            if (dzienWybrany == DateTime.Now.Date.AddDays(1))
            {
                dzienTygodnia = "Jutro, ";
            }

            return dzienTygodnia + dzienWybrany.Day + " " + miesiac + dzienWybrany.Year;
        }


        private void DodajNowyDzien(DateTime dzienWybrany, int klientID)
        {
            int count = _context.Dzien.Count(dz => dz.Dzien1 == dzienWybrany && dz.KlientId == klientID);
            if (count == 0)
            {
                var klient = _context.Klient.Where(k => k.KlientId == klientID);

                _context.Add(new Dzien()
                {
                    Dzien1 = dzienWybrany,
                    KlientId = klientID,
                    Sniadanie = klient.Select(k => k.Sniadanie).FirstOrDefault(),
                    Iisniadanie = klient.Select(k => k.Iisniadanie).FirstOrDefault(),
                    Obiad = klient.Select(k => k.Obiad).FirstOrDefault(),
                    Deser = klient.Select(k => k.Deser).FirstOrDefault(),
                    Przekaska = klient.Select(k => k.Przekaska).FirstOrDefault(),
                    Kolacja = klient.Select(k => k.Kolacja).FirstOrDefault(),
                    CelBialko = klient.Select(k => k.CelBialko).FirstOrDefault(),
                    CelTluszcze = klient.Select(k => k.CelTluszcze).FirstOrDefault(),
                    CelWegl = klient.Select(k => k.CelWegl).FirstOrDefault(),
                    CelKalorii = klient.Select(k => k.CelKalorii).FirstOrDefault(),
                    WypitaWoda = 0,
                }) ; 

                _context.SaveChanges();
            }

        }

        [HttpGet]
        public IActionResult ChangeDay(string day) 
        { 
            return RedirectToAction(nameof(OknoGlowne), new { dzienWybrany = Convert.ToDateTime(day) });
        }


        private void PoliczKalorie(int ktoryPosilek, string kcalPosilek, DateTime dzienWybrany, int klientID)
        {
            DodajNowyDzien(dzienWybrany, klientID);
            var posilek = _context.Posilek.Where(p => p.WKtorymPosilkuDnia == ktoryPosilek && p.Dzien.KlientId == klientID && p.Dzien.Dzien1 == dzienWybrany);
            List<double?> listaKalorie =  posilek.Select(p => p.Kalorie).ToList();
            double? kcal = 0;

            foreach(var item in listaKalorie)
            { kcal += item;}
            
            ViewData[kcalPosilek] = Math.Round((decimal)kcal, 0, MidpointRounding.AwayFromZero);
        }
  
        private void SprawdzPosilki(DateTime dzienWybrany, int klientID)
        {
            DodajNowyDzien(dzienWybrany, klientID);
            var dzien = _context.Dzien.Where(dz => dz.Dzien1 == dzienWybrany && dz.KlientId == klientID);

            ViewData["sniadanie"] = dzien.Select(dz => dz.Sniadanie).FirstOrDefault();
            ViewData["sniadanie2"] = dzien.Select(dz => dz.Iisniadanie).FirstOrDefault();
            ViewData["obiad"] = dzien.Select(dz => dz.Obiad).FirstOrDefault();
            ViewData["deser"] = dzien.Select(dz => dz.Deser).FirstOrDefault();
            ViewData["przekaska"] = dzien.Select(dz => dz.Przekaska).FirstOrDefault();
            ViewData["kolacja"] = dzien.Select(dz => dz.Kolacja).FirstOrDefault();
        }

        private void PoliczPostep(DateTime dzienWybrany, int klientID)
        {
            DodajNowyDzien(dzienWybrany, klientID);

            var posilki = _context.Posilek.Where(p => p.Dzien.KlientId == klientID && p.Dzien.Dzien1 == dzienWybrany);
            double? kcal = PoliczDana(posilki.Select(p => p.Kalorie).ToList());
            double? bialko = PoliczDana(posilki.Select(p => p.Bialko).ToList());
            double? tluszcze = PoliczDana(posilki.Select(p => p.Tluszcze).ToList());
            double? wegle = PoliczDana(posilki.Select(p => p.Weglowodany).ToList());

            var dzien = _context.Dzien.Where(d => d.KlientId == klientID && d.Dzien1 == dzienWybrany);
            double? celKalorii = dzien.Select(p => p.CelKalorii).FirstOrDefault();
            double? celBialko = dzien.Select(p => p.CelBialko).FirstOrDefault();
            double? celTluszcze = dzien.Select(p => p.CelTluszcze).FirstOrDefault();
            double? celWegle = dzien.Select(p => p.CelWegl).FirstOrDefault();

            ViewData["kalorie"] = kcal;
            ViewData["bialko"] = bialko;
            ViewData["wegle"] = wegle;
            ViewData["tluszcze"] = tluszcze;
            ViewData["kalorie0"] = Math.Round((decimal)kcal, 0, MidpointRounding.AwayFromZero);
            ViewData["bialko0"] = Math.Round((decimal)bialko, 0, MidpointRounding.AwayFromZero);
            ViewData["wegle0"] = Math.Round((decimal)wegle, 0, MidpointRounding.AwayFromZero);
            ViewData["tluszcze0"] = Math.Round((decimal)tluszcze, 0, MidpointRounding.AwayFromZero);
            ViewData["celkalorie"] = celKalorii;
            ViewData["celbialko"] = celBialko;
            ViewData["celtluszcze"] = celTluszcze;
            ViewData["celwegle"] = celWegle;
            ViewData["%kalorie"] = (int)(kcal / celKalorii * 100);
            ViewData["%wegle"] = (int)(wegle / celWegle * 100);
            ViewData["%tluszcze"] = (int)(tluszcze / celTluszcze * 100);
            ViewData["%bialko"] = (int)(bialko / celBialko * 100);
        }


        private double? PoliczDana(List<double?> lista)
        {
            double? dana = 0;
            foreach (var item in lista) { dana += item; }
            return dana;
        }

        private void WyswietlWode(DateTime dzienWybrany, int klientID)
        {
            var dzisiaj = _context.Dzien.Where(dz => dz.Dzien1 == dzienWybrany && dz.KlientId == klientID);
            ViewData["woda"] = dzisiaj.Select(dz => dz.WypitaWoda).FirstOrDefault();
        }

        public IActionResult DodajWode(int dzienID, int klientID)
        {
            DateTime dzienWybrany = (DateTime)_context.Dzien.Where(d => d.DzienId == dzienID).Select(d => d.Dzien1).FirstOrDefault();
            try {
                int dodanaWoda = int.Parse(HttpContext.Request.Form["DodanaWoda"]);
                var dzisiaj = _context.Dzien.First(dz => dz.Dzien1 == dzienWybrany && dz.KlientId == klientID);
                dzisiaj.WypitaWoda += dodanaWoda;
                _context.SaveChanges();
            }
            catch {}

            return RedirectToAction(nameof(OknoGlowne), new { klientID, dzienWybrany });
        }


        public IActionResult EdytujWode(int dzienID, int klientID)
        {
            DateTime dzienWybrany = (DateTime)_context.Dzien.Where(d => d.DzienId == dzienID).Select(d => d.Dzien1).FirstOrDefault();
            try {
                int edytowanaWoda = int.Parse(HttpContext.Request.Form["EdytowanaWoda"]);
                var dzisiaj = _context.Dzien.First(dz => dz.Dzien1 == dzienWybrany && dz.KlientId == klientID);
                dzisiaj.WypitaWoda = edytowanaWoda;
                _context.SaveChanges();
            }
            catch { }
            return RedirectToAction(nameof(OknoGlowne), new { klientID, dzienWybrany });
        }

     
        private void UstawPosilek(Posilek posilek, DateTime dzienWybrany, int klientID)
        {
            var produkt = _context.Produkt.Where(p => p.ProduktId == posilek.ProduktId);

            double? kcalW100gr = produkt.Select(p => p.Kalorie).FirstOrDefault();
            double? bialkoW100gr = produkt.Select(p => p.Bialko).FirstOrDefault();
            double? wegleW100gr = produkt.Select(p => p.Weglowodany).FirstOrDefault();
            double? tluszczeW100gr = produkt.Select(p => p.Tluszcze).FirstOrDefault();

            decimal kalorie = (decimal)(posilek.Gramatura * kcalW100gr / 100);
            decimal bialko = (decimal)(posilek.Gramatura * bialkoW100gr / 100);
            decimal wegle = (decimal)(posilek.Gramatura * wegleW100gr / 100);
            decimal tluszcze = (decimal)(posilek.Gramatura * tluszczeW100gr / 100);

            posilek.Kalorie = (double) Math.Round(kalorie, 1, MidpointRounding.AwayFromZero);
            posilek.Bialko = (double) Math.Round(bialko, 1, MidpointRounding.AwayFromZero);
            posilek.Weglowodany = (double) Math.Round(wegle, 1, MidpointRounding.AwayFromZero);
            posilek.Tluszcze = (double) Math.Round(tluszcze, 1, MidpointRounding.AwayFromZero);

            DodajNowyDzien(dzienWybrany, klientID);
            var dzisiaj = _context.Dzien.Where(dz => dz.Dzien1 == dzienWybrany && dz.KlientId == klientID);
            posilek.DzienId = dzisiaj.Select(dz => dz.DzienId).FirstOrDefault();

        }

        public IActionResult Add(int klientID, int dzienID, int wKtorym)
        {
            PrzeslijDaneDoWidoku(klientID, dzienID, wKtorym, 0);
            return View();
        }


        [HttpPost]
        public JsonResult Add(int wKtorym, int dzienID, int klientID, int gramatura, int produktID)
        {
            DateTime dzienWybrany = (DateTime)_context.Dzien.Where(d => d.DzienId == dzienID).Select(d => d.Dzien1).FirstOrDefault();
            Posilek posilek = new Posilek()
            {
                Gramatura = gramatura,
                ProduktId = produktID,
                WKtorymPosilkuDnia = wKtorym,
                DzienId = dzienID,
            };

            UstawPosilek(posilek, dzienWybrany, klientID);
            _context.Add(posilek);
            _context.SaveChanges();
            return Json(true);
        }


        [HttpPost]
        public IActionResult Edit(int posilekID, int gramatura)
        {
            var posilek = _context.Posilek.Where(p => p.PosilekId == posilekID).FirstOrDefault();
            var dzien = _context.Dzien.Where(d => posilek.DzienId == d.DzienId).FirstOrDefault();

            try
            { 
                if (posilek == null) { return NotFound(); }
                posilek.Gramatura = gramatura;
                UstawPosilek(posilek, (DateTime)dzien.Dzien1, dzien.KlientId);
                _context.Update(posilek);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PosilekExists(posilek.PosilekId))
                {
                    return NotFound();
                }
                else {
                    throw;
                }
            }
            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int posilekID)
        {
            var posilek = _context.Posilek.Where(p => p.PosilekId == posilekID).FirstOrDefault();
            if (posilek != null)
            {
                _context.Posilek.Remove(posilek);
                await _context.SaveChangesAsync();
            }
            return Json(false);
        }

        private bool PosilekExists(int id)
        {
            return _context.Posilek.Any(e => e.PosilekId == id);
        }
    }
}
