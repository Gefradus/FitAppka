using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitAppka.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace FitAppka.Controllers
{
    public class ProductController : Controller
    {
        private readonly FitAppContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(FitAppContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            return View(await _context.Produkt.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produkt = await _context.Produkt
                .FirstOrDefaultAsync(m => m.ProduktId == id);
            if (produkt == null)
            {
                return NotFound();
            }

            return View(produkt);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProduktId,NazwaProduktu,ZdjecieSciezka,Kalorie,Bialko,Tluszcze,Weglowodany,WitaminaA,WitaminaC,WitaminaD,WitaminaK,WitaminaE,WitaminaB1,WitaminaB2,WitaminaB5,WitaminaB6,Biotyna,KwasFoliowy,WitaminaB12,WitaminaPp,Cynk,Fosfor,Jod,Magnez,Miedz,Potas,Selen,Sod,Wapn,Zelazo")] Produkt produkt)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produkt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produkt);
        }


        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produkt = await _context.Produkt
                .FirstOrDefaultAsync(m => m.ProduktId == id);
            if (produkt == null)
            {
                return NotFound();
            }

            return View(produkt);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produkt = await _context.Produkt.FindAsync(id);
            _context.Produkt.Remove(produkt);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProduktExists(int id)
        {
            return _context.Produkt.Any(e => e.ProduktId == id);
        }
    }
}
