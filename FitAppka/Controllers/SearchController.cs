using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitAppka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using FitAppka.Repository;

namespace FitAppka.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class SearchController : Controller
    {
        
        private readonly IDayRepository _dayRepository;
        private readonly IProductRepository _productRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly FitAppContext _context;

        public SearchController(FitAppContext context, IWebHostEnvironment hostingEnvironment, IDayRepository dayRepository, IProductRepository productRepository, IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
            _dayRepository = dayRepository;
            _productRepository = productRepository;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index(string search, int inWhich, int dayID)
        {
            ViewData["wereSearched"] = false;
            if (search != null) { ViewData["wereSearched"] = true; }
       
            ViewData["clientID"] = _clientRepository.GetClientByLogin(User.Identity.Name).ClientId; 
            ViewData["dayID"] = dayID;
            ViewData["inWhich"] = inWhich;
            ViewData["search"] = search;
            ViewData["day"] = NapisDnia(dayID);
            ViewData["meal"] = MealName(inWhich);

            return View(await _context.Product.Where(p => p.ProductName.Contains(search)).ToListAsync());
        }

        private string MealName(int inWhichMeal)
        {
            if (inWhichMeal == 1) { return "Śniadanie"; }
            if (inWhichMeal == 2) { return "II śniadanie"; }
            if (inWhichMeal == 3) { return "Obiad"; }
            if (inWhichMeal == 4) { return "Deser"; }
            if (inWhichMeal == 5) { return "Przekąska"; }
            return "Kolacja";
        }

        private string NapisDnia(int dayID)
        {
            DateTime daySelected = _dayRepository.GetDayDateTime(dayID);
            string month = "";

            if (daySelected.Month == 1) { month = "sty"; }
            if (daySelected.Month == 2) { month = "lut"; }
            if (daySelected.Month == 3) { month = "mar"; }
            if (daySelected.Month == 4) { month = "kwi"; }
            if (daySelected.Month == 5) { month = "maj"; }
            if (daySelected.Month == 6) { month = "czer"; }
            if (daySelected.Month == 7) { month = "lip"; }
            if (daySelected.Month == 8) { month = "sie"; }
            if (daySelected.Month == 9) { month = "wrz"; }
            if (daySelected.Month == 10) { month = "paź"; }
            if (daySelected.Month == 11) { month = "lis"; }
            if (daySelected.Month == 12) { month = "gru"; }

            if (daySelected == DateTime.Now.Date){
                return "Dzisiaj";
            }
            else if (daySelected == DateTime.Now.Date.AddDays(-1)){
                return "Wczoraj";
            }
            else if (daySelected == DateTime.Now.Date.AddDays(1)){
                return "Jutro";
            }
            else {
                return daySelected.Day + " " + month;
            }
        }

      
        [HttpGet]
        public IActionResult Create(int inWhich, int dayID, int isAdmin)
        {
            CreateProductViewData(inWhich, dayID, isAdmin);
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateProductModel model, int dayID, int inWhich, int clientID, int isAdmin)
        {
            if (ModelState.IsValid)
            {     
                _productRepository.Add(CreateProductFromModel(model, CreatePathToPhoto(model)));
                
                if(isAdmin == 1) {
                    return RedirectToAction("AdminProduct", "Admin");
                } 
                else {
                    return RedirectToAction(nameof(Index), new { dayID, inWhich, clientID });
                }
            }

            CreateProductViewData(inWhich, dayID, isAdmin);
            return View(model); 
        }

        private void CreateProductViewData(int inWhich, int dayID, int isAdmin) 
        {
            ViewData["dayID"] = dayID;
            ViewData["inWhich"] = inWhich;
            ViewData["isAdmin"] = isAdmin;
        }


        private string CreatePathToPhoto(CreateProductModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string folder = Path.Combine(_hostingEnvironment.WebRootPath, "photos");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(folder, uniqueFileName);
                model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
            }
            return uniqueFileName;
        }


        private Product CreateProductFromModel(CreateProductModel model, string uniqueFileName)
        {
            return new Product()
            {
                ProductName = model.ProductName,
                PhotoPath = uniqueFileName,
                Calories = (double)model.Calories,
                Proteins = (double)model.Proteins,
                Fats = (double)model.Fats,
                Carbohydrates = (double)model.Carbohydrates,
                VitaminA = model.VitaminA,
                VitaminC = model.VitaminC,
                VitaminD = model.VitaminD,
                VitaminK = model.VitaminK,
                VitaminE = model.VitaminE,
                VitaminB1 = model.VitaminB1,
                VitaminB2 = model.VitaminB2,
                VitaminB6 = model.VitaminB6,
                Biotin = model.Biotin,
                VitaminB12 = model.VitaminB12,
                VitaminPp = model.VitaminPp,
                Zinc = model.Zinc,
                Phosphorus = model.Phosphorus,
                Iodine = model.Iodine,
                Magnesium = model.Magnesium,
                Copper = model.Copper,
                Potassium = model.Potassium,
                Selenium = model.Selenium,
                Sodium = model.Sodium,
                Calcium = model.Calcium,
                Iron = model.Iron
            };
        }
    }
}
