using Microsoft.AspNetCore.Mvc;
using FitAppka.Models;
using Microsoft.AspNetCore.Authorization;
using FitAppka.Repository;
using FitAppka.Service;
using FitAppka.Service.ServiceImpl;

namespace FitAppka.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IClientRepository _clientRepository;
        private readonly IProductManageService _productManageService;
        private readonly IContentRootPathHandlerService _contentRootService;
        private readonly FitAppContext _context;

        public ProductController(FitAppContext context, IClientRepository clientRepository, 
            IProductManageService productManageService, IContentRootPathHandlerService contentRootService)
        {
            _contentRootService = contentRootService;
            _clientRepository = clientRepository;
            _productManageService = productManageService;
            _context = context;
        }


        [HttpGet]
        public IActionResult Search(string search, int inWhich, int dayID, bool onlyUserItem, bool onlyFromDiet)
        {
            SearchProductViewData(search, inWhich, dayID);
            return View(_productManageService.SearchProduct(search, onlyUserItem, dayID, onlyFromDiet));
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
                return (isAdmin == 1 ? RedirectToAction("AdminProduct", "Admin") : RedirectToAction(nameof(Search), new { dayID, inWhich, clientID }));
            }

            CreateProductViewData(inWhich, dayID, isAdmin);
            return View(model); 
        }

        private void SearchProductViewData(string search, int inWhich, int dayID)
        {
            ViewData["dayID"] = dayID;
            ViewData["inWhich"] = inWhich;
            ViewData["search"] = search;
            ViewData["wereSearched"] = search != null;
            ViewData["day"] = _productManageService.DayPattern(dayID);
            ViewData["meal"] = _productManageService.MealName(inWhich);
            ViewData["path"] = _contentRootService.GetContentRootFileName();
        }

        private void CreateProductViewData(int inWhich, int dayID, int isAdmin) 
        {
            ViewData["dayID"] = dayID;
            ViewData["inWhich"] = inWhich;
            ViewData["isAdmin"] = isAdmin;
        }
    }
}
