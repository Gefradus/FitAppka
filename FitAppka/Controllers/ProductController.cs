using Microsoft.AspNetCore.Mvc;
using FitnessApp.Models;
using Microsoft.AspNetCore.Authorization;
using FitnessApp.Service;
using FitnessApp.Models.DTO;

namespace FitnessApp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductManageService _productManageService;

        public ProductController(IProductManageService productManageService)
        {
            _productManageService = productManageService;
        }

        [HttpGet]
        public IActionResult Search(SearchProductDTO dto)
        {
            return View(_productManageService.Dto(dto));
        }


        [HttpGet]
        public IActionResult Create(int atWhich, int dayId, int isAdmin)
        {
            CreateProductViewData(atWhich, dayId, isAdmin);
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductDTO model, int dayId, int atWhich, int isAdmin)
        {
            if (ModelState.IsValid)
            {     
                _productManageService.CreateProductFromModel(model);
                return isAdmin == 1 ? RedirectToAction("AdminProduct", "Admin") : RedirectToAction(nameof(Search), new { DayId = dayId, AtWhich = atWhich });
            }

            CreateProductViewData(atWhich, dayId, isAdmin);
            return View(model); 
        }


        private void CreateProductViewData(int atWhich, int dayId, int isAdmin) 
        {
            ViewData["dayId"] = dayId;
            ViewData["atWhich"] = atWhich;
            ViewData["isAdmin"] = isAdmin;
        }
    }
}
