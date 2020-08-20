using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FitAppka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitAppka.Controllers
{

    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class AdminController : Controller
    {
        private readonly FitAppContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminController(FitAppContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public IActionResult AdminHome()
        {
            if (GetLoggedInClient().CzyAdministrator)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
        }

        [HttpGet]
        public IActionResult AdminSettings()
        {
            if (GetLoggedInClient().CzyAdministrator)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
        }


        [HttpGet]
        public async Task<IActionResult> AdminProduct()
        {
            if (GetLoggedInClient().CzyAdministrator)
            {
                return View(await _context.Produkt.ToListAsync());
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
        }

        [HttpGet]
        public IActionResult AdminEditProduct(int id)
        {
            if (GetLoggedInClient().CzyAdministrator)
            {
                SendDataAboutProductToView(id);
                return View();
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdminEditProduct(CreateProductModel model, int produktID, int addOrEditPhoto)
        {
            if (ModelState.IsValid)
            {
                EditProduct(model, produktID, addOrEditPhoto);
                return RedirectToAction(nameof(AdminProduct));
            }
            return View(model);
        }

        private void EditProduct(CreateProductModel model, int id, int addOrEditPhoto)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Zdjecie != null)
                {
                    string folder = Path.Combine(_env.WebRootPath, "photos");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Zdjecie.FileName;
                    string filePath = Path.Combine(folder, uniqueFileName);
                    model.Zdjecie.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                Produkt produktTyp = _context.Produkt.Where(p => p.ProduktId == id).FirstOrDefault();

                produktTyp.NazwaProduktu = model.NazwaProduktu;
                produktTyp.Kalorie = model.Kalorie;
                produktTyp.Bialko = model.Bialko;
                produktTyp.Tluszcze = model.Tluszcze;
                produktTyp.Weglowodany = model.Weglowodany;
                produktTyp.WitaminaA = model.WitaminaA;
                produktTyp.WitaminaC = model.WitaminaC;
                produktTyp.WitaminaD = model.WitaminaD;
                produktTyp.WitaminaK = model.WitaminaK;
                produktTyp.WitaminaE = model.WitaminaE;
                produktTyp.WitaminaB1 = model.WitaminaB1;
                produktTyp.WitaminaB2 = model.WitaminaB2;
                produktTyp.WitaminaB5 = model.WitaminaB5;
                produktTyp.WitaminaB6 = model.WitaminaB6;
                produktTyp.Biotyna = model.Biotyna;
                produktTyp.WitaminaB12 = model.WitaminaB12;
                produktTyp.WitaminaPp = model.WitaminaPp;
                produktTyp.Cynk = model.Cynk;
                produktTyp.Fosfor = model.Fosfor;
                produktTyp.Jod = model.Jod;
                produktTyp.KwasFoliowy = model.KwasFoliowy;
                produktTyp.Magnez = model.Magnez;
                produktTyp.Miedz = model.Miedz;
                produktTyp.Potas = model.Potas;
                produktTyp.Selen = model.Selen;
                produktTyp.Sod = model.Sod;
                produktTyp.Wapn = model.Wapn;
                produktTyp.Zelazo = model.Zelazo;

                if(addOrEditPhoto == 0 || uniqueFileName != null)
                {
                    produktTyp.ZdjecieSciezka = uniqueFileName;
                }
 
                _context.Update(produktTyp);
                _context.SaveChanges();
            }
        }

        private void SendDataAboutProductToView(int id)
        {
            Produkt produktTyp = _context.Produkt.Where(p => p.ProduktId == id).FirstOrDefault();
            ViewData["produktID"] = id;
            ViewData["nazwa"] = produktTyp.NazwaProduktu;
            ViewData["sciezka"] = produktTyp.ZdjecieSciezka;
            ViewData["kalorie"] = produktTyp.Kalorie;
            ViewData["bialko"] = produktTyp.Bialko;
            ViewData["tluszcze"] = produktTyp.Tluszcze;
            ViewData["wegle"] = produktTyp.Weglowodany;
            ViewData["witA"] = produktTyp.WitaminaA;
            ViewData["witC"] = produktTyp.WitaminaC;
            ViewData["witD"] = produktTyp.WitaminaD;
            ViewData["witK"] = produktTyp.WitaminaK;
            ViewData["witE"] = produktTyp.WitaminaE;
            ViewData["witB1"] = produktTyp.WitaminaB1;
            ViewData["witB2"] = produktTyp.WitaminaB2;
            ViewData["witB5"] = produktTyp.WitaminaB5;
            ViewData["witB6"] = produktTyp.WitaminaB6;
            ViewData["biotyna"] = produktTyp.Biotyna;
            ViewData["witB12"] = produktTyp.WitaminaB12;
            ViewData["witPP"] = produktTyp.WitaminaPp;
            ViewData["cynk"] = produktTyp.Cynk;
            ViewData["fosfor"] = produktTyp.Fosfor;
            ViewData["jod"] = produktTyp.Jod;
            ViewData["kwasFoliowy"] = produktTyp.KwasFoliowy;
            ViewData["magnez"] = produktTyp.Magnez;
            ViewData["miedz"] = produktTyp.Miedz;
            ViewData["potas"] = produktTyp.Potas;
            ViewData["selen"] = produktTyp.Selen;
            ViewData["sod"] = produktTyp.Sod;
            ViewData["wapn"] = produktTyp.Wapn;
            ViewData["zelazo"] = produktTyp.Zelazo;

            if(produktTyp.ZdjecieSciezka == null)
            {
                ViewData["addOrEdit"] = 0;
            }
            else
            {
                ViewData["addOrEdit"] = 1;
            }

        }

        
        [HttpPost]
        public IActionResult Delete(int productID)
        {
            Produkt product = _context.Produkt.Where(p => p.ProduktId == productID).FirstOrDefault();
            UsunPrzypisanePosilki(productID);
            
            if (product != null)
            {
                _context.Produkt.Remove(product);
                _context.SaveChanges();
            }
            return Json(false);
        }


        [HttpGet]
        public async Task<IActionResult> AdminClient()
        {
            if (GetLoggedInClient().CzyAdministrator)
            {
                return View(await _context.Klient.ToListAsync());
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
        }

        [HttpGet]
        public async Task<IActionResult> AdminTraining()
        {
            if (GetLoggedInClient().CzyAdministrator)
            {
                return View(await _context.TreningRodzaj.ToListAsync());
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
        }


        private Klient GetLoggedInClient()
        {
            return _context.Klient.Where(k => k.Login.ToLower().Equals(User.Identity.Name.ToLower())).FirstOrDefault();
        }

        private void UsunPrzypisanePosilki(int productID){
            List<Posilek> przypisanePosilki = _context.Posilek.Where(p => p.ProduktId == productID).ToList();

            foreach(var posilek in przypisanePosilki){
                _context.Posilek.Remove(posilek);
            }
            _context.SaveChanges();
        }


        
    }
}
