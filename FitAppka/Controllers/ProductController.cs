using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitAppka.Models;
using Microsoft.AspNetCore.Authorization;
using FitAppka.Repository;
using FitAppka.Service;

namespace FitAppka.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IClientRepository _clientRepository;
        private readonly IProductManageService _productManageService;
        private readonly FitAppContext _context;

        public ProductController(FitAppContext context, IClientRepository clientRepository, IProductManageService productManageService)
        {
            _clientRepository = clientRepository;
            _productManageService = productManageService;
            _context = context;
        }

        // [HttpGet]
        // public async Task<IActionResult> Search(string search, int inWhich, int dayID, bool onlyUserItem) 
        // {
        //     SearchProductViewData(search, inWhich, dayID);
        //     return View(FindProducts(search, onlyUserItem));
        // }

        // private List<Product> FindProducts(string search, bool onlyUserItem){
        //     List<Product> products = _context.Product.Where(p => p.ProductName.Contains(search)).ToListAsync();
        //     if(onlyUserItem){
        //         return products.Where(p => p.ClientId == _clientRepository.GetLoggedInClientId());
        //     }
        //     return products;
        // }

        [HttpGet]
        public async Task<IActionResult> Search2(string search, int inWhich, int dayID)
        {
            SearchProductViewData(search, inWhich, dayID);
            return View(await _context.Product.Where(p => p.ProductName.Contains(search) || string.IsNullOrEmpty(search)).ToListAsync());
        }


        [HttpGet]
        public IActionResult Create(int inWhich, int dayID, int isAdmin)
        {
            CreateProductViewData(inWhich, dayID, isAdmin);
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductDTO model, int dayID, int inWhich, int clientID, int isAdmin)
        {
            if (ModelState.IsValid)
            {     
                _productManageService.CreateProductFromModel(model);
                
                if(isAdmin == 1) {
                    return RedirectToAction("AdminProduct", "Admin");
                } 
                else {
                    return RedirectToAction(nameof(Search2), new { dayID, inWhich, clientID });
                }
            }

            CreateProductViewData(inWhich, dayID, isAdmin);
            return View(model); 
        }

        private void SearchProductViewData(string search, int inWhich, int dayID)
        {
            ViewData["wereSearched"] = false;
            if (search != null) { ViewData["wereSearched"] = true; }

            ViewData["dayID"] = dayID;
            ViewData["inWhich"] = inWhich;
            ViewData["search"] = search;
            ViewData["day"] = _productManageService.DayPattern(dayID);
            ViewData["meal"] = _productManageService.MealName(inWhich);
        }

        private void CreateProductViewData(int inWhich, int dayID, int isAdmin) 
        {
            ViewData["dayID"] = dayID;
            ViewData["inWhich"] = inWhich;
            ViewData["isAdmin"] = isAdmin;
        }
    }
}
