using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitAppka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitAppka.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly FitAppContext _context;
        private readonly SettingsServices service;

        public SettingsController(FitAppContext context)
        {
            service = new SettingsServices();
            _context = context;
        }

        [HttpGet]
        public IActionResult Settings()
        {
            var klientID = _context.Klient.Where(k => k.Login == User.Identity.Name).Select(k => k.KlientId).FirstOrDefault();
            FirstAppRun(klientID);

            ViewData["klientID"] = klientID;
            return View();
        }

        private void FirstAppRun(int klientID)
        {
            try     //jeśli try się powiedzie znaczy to że użytkownik już podał dane
            {
                Klient klient = _context.Klient.Where(k => k.KlientId == klientID).FirstOrDefault();
                IQueryable<PomiarWagi> listaPomiarow = _context.PomiarWagi.Where(p => p.KlientId == klientID);

                var data = klient.DataUrodzenia.Value;
                ViewData["dataUrodzenia"] = data.ToString("yyyy-MM-dd");
                ViewData["waga"] = UstawOstatniPomiarWagi(listaPomiarow, ZabezpieczeniePrzedMinimum(listaPomiarow, klientID));
                ViewData["wzrost"] = klient.Wzrost;
                ViewData["celZmian"] = (int)klient.CelZmianWagi;
                ViewData["aktywnosc"] = (int)klient.PoziomAktywnosci;
                ViewData["tempo"] = klient.TempoZmian.ToString().Replace(',', '.');
                ViewData["czyPierwsze"] = 0;

                UstawBoolean(klient.Plec, "plec");
                UstawBoolean(klient.Sniadanie, "sniadanie");
                UstawBoolean(klient.Iisniadanie, "2sniadanie");
                UstawBoolean(klient.Obiad, "obiad");
                UstawBoolean(klient.Deser, "deser");
                UstawBoolean(klient.Przekaska, "przekaska");
                UstawBoolean(klient.Kolacja, "kolacja");
            }
            catch   //użytkownik nie podał danych (pierwsze uruchomienie)
            {
                ViewData["dataUrodzenia"] = DateTime.Now.ToString("yyyy-MM-dd");
                ViewData["czyPierwsze"] = 1;
            }
        }

        private double UstawOstatniPomiarWagi(IQueryable<PomiarWagi> listaPomiarow, DateTime? dataPomiaru)
        {
                double ostatniPomiarWagi = 0;
                foreach (var item in listaPomiarow)
                {
                    if (dataPomiaru == item.DataPomiaru)
                    {
                        ostatniPomiarWagi = item.Waga;
                    }
                }

                return ostatniPomiarWagi;
        }


        private DateTime? ZabezpieczeniePrzedMinimum(IQueryable<PomiarWagi> listaPomiarow, int klientID)
        {
                
                DateTime? dataPomiaru = DateTime.MinValue;

                foreach (PomiarWagi pomiarWagi in listaPomiarow)
                {
                    if (dataPomiaru < pomiarWagi.DataPomiaru)
                    {
                        dataPomiaru = pomiarWagi.DataPomiaru;
                    }
                }

                return dataPomiaru;
        }


        private void UstawBoolean(bool? flaga, string nazwaViewDaty)
        {
            if ((bool)flaga)
            {
                ViewData[nazwaViewDaty] = 1;
            }
            else
            {
                ViewData[nazwaViewDaty] = 0;
            }
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Settings(FirstAppRunModel m, int czyPierwsze)
        {
            if (ModelState.IsValid)
            {
                var klient = _context.Klient.Where(k => k.Login == User.Identity.Name).FirstOrDefault();
                int zapotrzebowanie = service.ObliczZapotrzebowanie((bool)m.Plec, m.DataUrodzenia, m.Wzrost, (double)m.Waga, service.PoziomAktywnosci(m.Aktywnosc));

                double tempo = 0.4;
                try
                {
                    tempo = double.Parse(m.TempoZmian.Replace('.', ',').Replace(" ", ""));
                }
                catch { }

                int celKalorii = service.ObliczCelKalorii(zapotrzebowanie, m.Cel, tempo);
                int celBialko = service.ObliczCelBialko(m.Waga, celKalorii, m.Aktywnosc);
                int celTluszcze = service.ObliczCelTluszcze(celKalorii);
                int celWegle = service.ObliczCelWegle(celKalorii, celBialko, celTluszcze);

                klient.CelKalorii = celKalorii;
                klient.ZapotrzebowanieKcal = zapotrzebowanie;
                klient.CelBialko = celBialko;
                klient.CelTluszcze = celTluszcze;
                klient.CelWegl = celWegle;
                klient.TempoZmian = tempo;
                if (czyPierwsze == 1) //sprawdzamy czy to pierwsze uruchomienie
                {
                    klient.DataDolaczenia = DateTime.Now.Date;
                }
                else
                {
                    List<int> listaDniOdDzisID = new List<int>();
                    foreach (var item in _context.Dzien.Where(d => d.KlientId == klient.KlientId))
                    {
                        if (item.Dzien1 >= DateTime.Now.Date)
                        {
                            listaDniOdDzisID.Add(item.DzienId);
                        }
                    }

                    foreach (var dzienOdDzisID in listaDniOdDzisID)
                    {
                        foreach (var dzienOdDzis in _context.Dzien)
                        {
                            if (dzienOdDzis.DzienId == dzienOdDzisID)
                            {
                                dzienOdDzis.CelKalorii = celKalorii;
                                dzienOdDzis.CelBialko = celBialko;
                                dzienOdDzis.CelTluszcze = celTluszcze;
                                dzienOdDzis.CelWegl = celWegle;
                                dzienOdDzis.Sniadanie = m.Sniadanie;
                                dzienOdDzis.Iisniadanie = m.Iisniadanie;
                                dzienOdDzis.Obiad = m.Obiad;
                                dzienOdDzis.Deser = m.Deser;
                                dzienOdDzis.Przekaska = m.Przekaska;
                                dzienOdDzis.Kolacja = m.Kolacja;
                                if (m.Sniadanie == null) { dzienOdDzis.Sniadanie = false; }
                                if (m.Iisniadanie == null) { dzienOdDzis.Iisniadanie = false; }
                                if (m.Obiad == null) { dzienOdDzis.Obiad = false; }
                                if (m.Deser == null) { dzienOdDzis.Deser = false; }
                                if (m.Przekaska == null) { dzienOdDzis.Przekaska = false; }
                                if (m.Kolacja == null) { dzienOdDzis.Kolacja = false; }
                            }
                        }
                    }
                }

                klient.DataUrodzenia = m.DataUrodzenia;
                klient.Plec = m.Plec;
                klient.Wzrost = m.Wzrost;
                klient.Sniadanie = m.Sniadanie;
                klient.Iisniadanie = m.Iisniadanie;
                klient.Obiad = m.Obiad;
                klient.Deser = m.Deser;
                klient.Przekaska = m.Przekaska;
                klient.Kolacja = m.Kolacja;
                klient.CelZmianWagi = m.Cel;
                klient.PoziomAktywnosci = m.Aktywnosc;

                if (m.Sniadanie == null) { klient.Sniadanie = false; }
                if (m.Iisniadanie == null) { klient.Iisniadanie = false; }
                if (m.Obiad == null) { klient.Obiad = false; }
                if (m.Deser == null) { klient.Deser = false; }
                if (m.Przekaska == null) { klient.Przekaska = false; }
                if (m.Kolacja == null) { klient.Kolacja = false; }

                klient.PomiarWagi.Add(new PomiarWagi()
                {
                    DataPomiaru = DateTime.Now,
                    Waga = (double)m.Waga,

                });

                _context.Update(klient);

                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Start", "Posilek", new { id = klient.KlientId });
                }
                catch
                {
                    ModelState.AddModelError("", "Data urodzenia nie może być mniejsza niż 01.01.1900r.");
                    return View(m);
                }

            }

            return View(m);
        }
    }

}

