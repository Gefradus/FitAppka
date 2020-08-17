using FitAppka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitAppka.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class TrainingController : Controller
    {

        private readonly FitAppContext _context;

        public TrainingController(FitAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> TrainingPanel(int dzienID)
        {
            dzienID = DajDzisJesliNieWybranoDnia(dzienID);
            ViewData["dzien"] = DajDzienPoID(dzienID);
            ViewData["dzienID"] = dzienID;
            ViewData["klientID"] = DajZalogowanegoKlientaID();
            ViewData["spaloneKcal"] = KcalSpaloneWDniu(dzienID);
            ViewData["czasTreningow"] = CzasTreningowWDniu(dzienID);
            ViewData["celKcal"] = DajCelKcalWDniu(dzienID);
            ViewData["celMinut"] = DajCelMinutTreningowWDniu(dzienID);
            return View(await _context.Trening.Include(t => t.TreningRodzaj).Include(t => t.Dzien).ToListAsync());
        }

        [HttpGet]
        public IActionResult ChangeDay(string day)
        {
            return RedirectToAction(nameof(TrainingPanel), new { dzienID = DajObecnyDzienID(Convert.ToDateTime(day)) });
        }

        private int DajCelKcalWDniu(int dzienID)
        {
            int? celKcal = DajDzien(dzienID).CelSpalonychKcal;
            if(celKcal != null)
            {
                return (int) celKcal;
            }
            else
            {
                return 0;
            }

        }

        private int DajCelMinutTreningowWDniu(int dzienID)
        {
            int? celMinut = DajDzien(dzienID).CelMinTreningow;
            if (celMinut != null)
            {
                return (int)celMinut;
            }
            else
            {
                return 0;
            }
        }

        private Dzien DajDzien(int dzienID)
        {
            return _context.Dzien.Where(d => d.DzienId.Equals(dzienID)).FirstOrDefault();
        }

        private int KcalSpaloneWDniu(int dzienID)
        {
            List<Trening> treningi = _context.Trening.Where(t => t.DzienId.Equals(dzienID)).ToList();
            int? kcal = 0;
            foreach (Trening trening in treningi) {
                kcal += trening.SpaloneKalorie;
            }

            return (int) kcal;
        }

        private int DajDzisJesliNieWybranoDnia(int dzienID)
        {
            if(dzienID == 0)
            {
                return DajDzisiajID();
            }
            else
            {
                return dzienID;
            }
        }

        private int CzasTreningowWDniu(int dzienID)
        {
            List<Trening> treningi = _context.Trening.Where(t => t.DzienId.Equals(dzienID)).ToList();
            int? czas = 0;
            foreach (Trening trening in treningi)
            {
                czas += trening.CzasWMinutach;
            }

            return (int)czas;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string search, int dzienID)
        {
            ViewData["dzienID"] = dzienID;
            SprawdzCzySzukano(search);
            return View(await _context.TreningRodzaj.Where(t => t.NazwaTreningu.Contains(search)).ToListAsync());
        }

        [HttpPost]
        public IActionResult Search(int dzienID, int treningTypId, int czasWMinutach, int spaloneKcal)
        {
            _context.Add(new Trening()
            {
                DzienId = dzienID,
                CzasWMinutach = czasWMinutach,
                TreningRodzajId = treningTypId,
                SpaloneKalorie = spaloneKcal
            });

            try
            {
                _context.SaveChanges();
                return RedirectToAction(nameof(TrainingPanel), new { dzienID });
            }
            catch { return RedirectToAction(nameof(TrainingPanel), new { dzienID = DajDzisiajID() }); } 
        }

        [HttpPost]
        public async Task<IActionResult> UsunTrening(int treningID)
        {
            var trening = _context.Trening.Where(t => t.TreningId == treningID).FirstOrDefault();
           
            if (trening != null)
            {
                _context.Trening.Remove(trening);
                await _context.SaveChangesAsync();
            }
            return Json(false);
        }

        [HttpPost]
        public IActionResult AddTraining(int dzienID, string nazwa, int wydatek)
        {
            _context.Add(new TreningRodzaj
            {
                NazwaTreningu = nazwa,
                KcalNaMin = wydatek
            });

            _context.SaveChanges();

            return RedirectToAction(nameof(Search), new { dzienID });
        }


        private bool TrainingExists(int id)
        {
            return _context.Trening.Any(t => t.TreningId == id);
        }

        private int DajObecnyDzienID(DateTime dzien)
        {
            int id = DajDzienPoDacie(dzien);
            if (id != 0)
            {
                return id;
            }
            else
            {
                DodajNowyDzien(dzien);
                return DajDzienPoDacie(dzien);
            }

        }

        private void DodajNowyDzien(DateTime dzien)
        {
            var klient = _context.Klient.Where(k => k.KlientId == DajZalogowanegoKlientaID());

            _context.Add(new Dzien(){
                Dzien1 = dzien,
                KlientId = DajZalogowanegoKlientaID(),
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
            });

            _context.SaveChanges();
        }


        private int DajDzienPoDacie(DateTime dzien)
        {
            return _context.Dzien.Where(d => d.KlientId == DajZalogowanegoKlientaID() && d.Dzien1 == dzien.Date).Select(d => d.DzienId).FirstOrDefault();
        }


        private int DajDzisiajID()
        {
            return DajObecnyDzienID(DateTime.Now);
        }

        private string DajDzienPoID(int dzienID)
        {
            DateTime dateTime = (DateTime) _context.Dzien.Where(d => d.DzienId == dzienID && d.KlientId == DajZalogowanegoKlientaID()).Select(d => d.Dzien1).FirstOrDefault();
            return dateTime.Date.ToString("dd.MM.yyyy");
        }

        private void SprawdzCzySzukano(string search){
            ViewData["czySzukano"] = false;
        
            if (search != null)
            {
                ViewData["czySzukano"] = true;
            }
        }

        private int DajZalogowanegoKlientaID(){
            return _context.Klient.Where(k => k.Login.ToLower() == User.Identity.Name.ToLower()).Select(k => k.KlientId).FirstOrDefault();
        }


        private string NazwaPosilku(int wKtorym)
        {
            if (wKtorym == 1) { return "Śniadanie"; }
            if (wKtorym == 2) { return "II śniadanie"; }
            if (wKtorym == 3) { return "Obiad"; }
            if (wKtorym == 4) { return "Deser"; }
            if (wKtorym == 5) { return "Przekąska"; }
            return "Kolacja";
        }

    }
}
