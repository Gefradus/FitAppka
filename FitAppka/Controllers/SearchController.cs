using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitAppka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace FitAppka.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class SearchController : Controller
    {
        private readonly FitAppContext _context;
        private readonly IWebHostEnvironment hostingEnvironment;

        public SearchController(FitAppContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index(string search, int klientID, int wKtorym, int dzienID)
        {
            if (klientID == 0 || wKtorym == 0 || dzienID == 0)
            {
                klientID = _context.Klient.Where(k => k.Login.ToLower() == User.Identity.Name.ToLower()).Select(k => k.KlientId).FirstOrDefault();
                return RedirectToAction("OknoGlowne", "Posilek", new { klientID, dzienWybrany = DateTime.Now.Date });
            }

            if (User.Identity.Name.ToLower() != _context.Klient.Where(k => k.KlientId == klientID).Select(k => k.Login.ToLower()).FirstOrDefault())
            {
                return RedirectToAction("Logout", "Login");  //zabezpieczenie przed wchodzeniem na obce konto
            }
            ViewData["czySzukano"] = false;

            if (search != null)
            {
                ViewData["czySzukano"] = true;
            }
       
            ViewData["klientID"] = klientID;
            ViewData["dzienID"] = dzienID;
            ViewData["wKtorym"] = wKtorym;
            ViewData["search"] = search;
            ViewData["klientImie"] = ImieINaziwsko(klientID);
            ViewData["dzien"] = NapisDnia(dzienID);
            ViewData["posilek"] = NazwaPosilku(wKtorym);

            return View(await _context.Produkt.Where(p => p.NazwaProduktu.Contains(search)).ToListAsync());
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

        private string NapisDnia(int dzienID)
        {
            DateTime dzienWybrany = (DateTime) _context.Dzien.Where(d => d.DzienId == dzienID).Select(d => d.Dzien1).FirstOrDefault();
            string miesiac = "";

            if (dzienWybrany.Month == 1) { miesiac = "sty"; }
            if (dzienWybrany.Month == 2) { miesiac = "lut"; }
            if (dzienWybrany.Month == 3) { miesiac = "mar"; }
            if (dzienWybrany.Month == 4) { miesiac = "kwi"; }
            if (dzienWybrany.Month == 5) { miesiac = "maj"; }
            if (dzienWybrany.Month == 6) { miesiac = "czer"; }
            if (dzienWybrany.Month == 7) { miesiac = "lip"; }
            if (dzienWybrany.Month == 8) { miesiac = "sie"; }
            if (dzienWybrany.Month == 9) { miesiac = "wrz"; }
            if (dzienWybrany.Month == 10) { miesiac = "paź"; }
            if (dzienWybrany.Month == 11) { miesiac = "lis"; }
            if (dzienWybrany.Month == 12) { miesiac = "gru"; }

            if (dzienWybrany == DateTime.Now.Date){
                return "Dzisiaj";
            }
            else if (dzienWybrany == DateTime.Now.Date.AddDays(-1)){
                return "Wczoraj";
            }
            else if (dzienWybrany == DateTime.Now.Date.AddDays(1)){
                return "Jutro";
            }
            else{
                return dzienWybrany.Day + " " + miesiac;
            }
        }

        private string ImieINaziwsko(int klientID)
        {
            var klient = _context.Klient.Where(k => k.KlientId == klientID);
            return klient.Select(k => k.Imie).FirstOrDefault() + " " + klient.Select(k => k.Nazwisko).FirstOrDefault();
        }

        public IActionResult Create(int klientID, int wKtorym, int dzienID, int czyAdmin)
        {
            ViewData["dzienID"] = dzienID;
            ViewData["wKtorym"] = wKtorym;
            ViewData["klientID"] = klientID;
            ViewData["czyAdmin"] = czyAdmin;
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateProductModel model, int dzienID, int wKtorym, int klientID, int czyAdmin)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Zdjecie != null) {
                    string folder = Path.Combine(hostingEnvironment.WebRootPath, "photos");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Zdjecie.FileName;
                    string filePath = Path.Combine(folder, uniqueFileName);
                    model.Zdjecie.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                Produkt produktTyp = new Produkt() {
                       NazwaProduktu = model.NazwaProduktu,
                       ZdjecieSciezka = uniqueFileName,
                        Kalorie = model.Kalorie,
                        Bialko = model.Bialko,
                        Tluszcze = model.Tluszcze,
                        Weglowodany = model.Weglowodany,
                        WitaminaA = model.WitaminaA,
                        WitaminaC = model.WitaminaC,
                        WitaminaD = model.WitaminaD,
                        WitaminaK = model.WitaminaK,
                        WitaminaE = model.WitaminaE,
                        WitaminaB1 = model.WitaminaB1,
                        WitaminaB2 = model.WitaminaB2,
                        WitaminaB6 = model.WitaminaB6,
                        Biotyna = model.Biotyna,
                        WitaminaB12 = model.WitaminaB12,
                        WitaminaPp = model.WitaminaPp,
                        Cynk = model.Cynk,
                        Fosfor = model.Fosfor,
                        Jod = model.Jod,
                        Magnez = model.Magnez,
                        Miedz = model.Miedz,
                        Potas = model.Potas,
                        Selen = model.Selen,
                        Sod = model.Sod,
                        Wapn = model.Wapn,
                        Zelazo = model.Zelazo
                };

                _context.Add(produktTyp);
                _context.SaveChanges();

                if(czyAdmin == 1) {
                    return RedirectToAction("AdminProduct", "Admin");
                } 
                else 
                {
                    return RedirectToAction(nameof(Index), new { dzienID, wKtorym, klientID });
                }
                

            }
            return View(model);
        }

        // GET: Search/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produktTyp = await _context.Produkt.FindAsync(id);
            if (produktTyp == null)
            {
                return NotFound();
            }
            return View(produktTyp);
        }

        // POST: Search/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Produkt produkt)
        {
            if (id != produkt.ProduktId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produkt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduktTypExists(produkt.ProduktId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(produkt);
        }

        // GET: Search/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produktTyp = await _context.Produkt
                .FirstOrDefaultAsync(m => m.ProduktId == id);
            if (produktTyp == null)
            {
                return NotFound();
            }

            return View(produktTyp);
        }

        // POST: Search/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produktTyp = await _context.Produkt.FindAsync(id);
            _context.Produkt.Remove(produktTyp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProduktTypExists(int id)
        {
            return _context.Produkt.Any(e => e.ProduktId == id);
        }
    }
}
