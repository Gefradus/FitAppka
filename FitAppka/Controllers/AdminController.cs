using System.Linq;
using System.Threading.Tasks;
using FitAppka.Models;
using FitAppka.Repository;
using FitAppka.Service;
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
        private readonly IProductManageService _productManageService;
        private readonly IClientRepository _clientRepository;
        private readonly FitAppContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminController(FitAppContext context, IWebHostEnvironment env, IProductRepository productRepository, 
            IClientRepository clientRepository, IProductManageService productManageService)
        {
            _productManageService = productManageService;
            _clientRepository = clientRepository;
            _productRepository = productRepository;
            _context = context;
            _env = env;
        }

        [HttpGet]
        public IActionResult AdminHome()
        {
            if (_clientRepository.IsLoggedInClientAdmin()) {
                return View();
            }
            else {
                return RedirectToAction("Logout", "Login");
            }
        }

        [HttpGet]
        public IActionResult AdminSettings()
        {
            if (_clientRepository.IsLoggedInClientAdmin()) {
                return View();
            }
            else {
                return RedirectToAction("Logout", "Login");
            }
        }


        [HttpGet]
        public async Task<IActionResult> AdminProduct(string search)
        {
            if (_clientRepository.IsLoggedInClientAdmin()) {
                if(search != null) {
                    return View(await _context.Product.Where(p => p.ProductName.Contains(search)).ToListAsync());
                } 
                else {
                    return View(await _context.Product.ToListAsync());
                }
            }
            else {
                return RedirectToAction("Logout", "Login");
            }
        }

        [HttpGet]
        public IActionResult AdminEditProduct(int id)
        {
            if (_clientRepository.IsLoggedInClientAdmin())
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
        public IActionResult AdminEditProduct(CreateProductModel model, int productID, int addOrEditPhoto)
        {
            if (ModelState.IsValid)
            {
                _productManageService.UpdateProduct(model, productID, addOrEditPhoto);
                return RedirectToAction(nameof(AdminProduct));
            }
            return View(model);
        }

        private void SendDataAboutProductToView(int id)
        {
            Product product = _productRepository.GetProduct(id);
            ViewData["productID"] = id;
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

            if (product.PhotoPath == null) {
                ViewData["addOrEdit"] = 0;
            }
            else {
                ViewData["addOrEdit"] = 1;
            }

        }

        [HttpPost]
        public JsonResult Delete(int productID)
        {
            if (_clientRepository.IsLoggedInClientAdmin())
            {
                _productRepository.Delete(productID);
                return Json(true);
            }
            return Json(false);
        }


        [HttpGet]
        public async Task<IActionResult> AdminClient()
        {
            if (_clientRepository.IsLoggedInClientAdmin())
            {
                return View(await _clientRepository.GetAllClientsAsync());
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
        }


        
    }
}
