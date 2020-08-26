using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FitAppka.Models;
using FitAppka.Repository;
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
        private readonly IProductRepository _productRepository;
        private readonly IClientRepository _clientRepository;
        private readonly FitAppContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminController(FitAppContext context, IWebHostEnvironment env, IProductRepository productRepository, IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
            _productRepository = productRepository;
            _context = context;
            _env = env;
        }

        [HttpGet]
        public IActionResult AdminHome()
        {
            if (GetLoggedInClient().IsAdmin)
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
            if (GetLoggedInClient().IsAdmin)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
        }


        [HttpGet]
        public async Task<IActionResult> AdminProduct(string search)
        {
            if (GetLoggedInClient().IsAdmin)
            {
                if(search != null) {
                    return View(await _context.Product.Where(p => p.ProductName.Contains(search)).ToListAsync());
                } 
                else {
                    return View(await _context.Product.ToListAsync());
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
        }

        [HttpGet]
        public IActionResult AdminEditProduct(int id)
        {
            if (GetLoggedInClient().IsAdmin)
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
                if (model.Photo != null)
                {
                    string folder = Path.Combine(_env.WebRootPath, "photos");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(folder, uniqueFileName);
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                Product product = _productRepository.GetProduct(id);

                product.ProductName = model.ProductName;
                product.Calories = (double) model.Calories;
                product.Proteins = (double) model.Proteins;
                product.Fats = (double) model.Fats;
                product.Carbohydrates = (double) model.Carbohydrates;
                product.VitaminA = model.VitaminA;
                product.VitaminC = model.VitaminC;
                product.VitaminD = model.VitaminD;
                product.VitaminK = model.VitaminK;
                product.VitaminE = model.VitaminE;
                product.VitaminB1 = model.VitaminB1;
                product.VitaminB2 = model.VitaminB2;
                product.VitaminB5 = model.VitaminB5;
                product.VitaminB6 = model.VitaminB6;
                product.Biotin = model.Biotin;
                product.VitaminB12 = model.VitaminB12;
                product.VitaminPp = model.VitaminPp;
                product.Zinc = model.Zinc;
                product.Phosphorus = model.Phosphorus;
                product.Iodine = model.Iodine;
                product.FolicAcid = model.FolicAcid;
                product.Magnesium = model.Magnesium;
                product.Copper = model.Copper;
                product.Potassium = model.Potassium;
                product.Selenium = model.Selenium;
                product.Sodium = model.Sodium;
                product.Calcium = model.Calcium;
                product.Iron = model.Iron;

                if(addOrEditPhoto == 0 || uniqueFileName != null)
                {
                    product.PhotoPath = uniqueFileName;
                }

                _productRepository.Update(product);
            }
        }

        private void SendDataAboutProductToView(int id)
        {
            Product product = _productRepository.GetProduct(id);
            ViewData["produktID"] = id;
            ViewData["name"] = product.ProductName;
            ViewData["path"] = product.PhotoPath;
            ViewData["calories"] = product.Calories;
            ViewData["proteins"] = product.Proteins;
            ViewData["fats"] = product.Fats;
            ViewData["carbs"] = product.Carbohydrates;
            ViewData["vitA"] = product.VitaminA;
            ViewData["vitC"] = product.VitaminC;
            ViewData["vitD"] = product.VitaminD;
            ViewData["vitK"] = product.VitaminK;
            ViewData["vitE"] = product.VitaminE;
            ViewData["vitB1"] = product.VitaminB1;
            ViewData["vitB2"] = product.VitaminB2;
            ViewData["vitB5"] = product.VitaminB5;
            ViewData["vitB6"] = product.VitaminB6;
            ViewData["biotin"] = product.Biotin;
            ViewData["vitB12"] = product.VitaminB12;
            ViewData["vitPP"] = product.VitaminPp;
            ViewData["zinc"] = product.Zinc;
            ViewData["phosphorus"] = product.Phosphorus;
            ViewData["iodine"] = product.Iodine;
            ViewData["folicAcid"] = product.FolicAcid;
            ViewData["magnesium"] = product.Magnesium;
            ViewData["copper"] = product.Copper;
            ViewData["potassium"] = product.Potassium;
            ViewData["selenium"] = product.Selenium;
            ViewData["sodium"] = product.Sodium;
            ViewData["calcium"] = product.Calcium;
            ViewData["iron"] = product.Iron;

            if(product.PhotoPath == null)
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
            _productRepository.Delete(productID);
            return Json(false);
        }


        [HttpGet]
        public async Task<IActionResult> AdminClient()
        {
            if (GetLoggedInClient().IsAdmin)
            {
                return View(await _context.Client.ToListAsync());
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
        }

        private Client GetLoggedInClient()
        {
            return _clientRepository.GetClientByLogin(User.Identity.Name);
        }
        
    }
}
