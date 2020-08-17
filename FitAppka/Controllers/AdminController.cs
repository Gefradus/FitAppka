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
    public class AdminController : Controller
    {
        private readonly FitAppContext _context;

        public AdminController(FitAppContext context)
        {
            _context = context;
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

        
    }
}
