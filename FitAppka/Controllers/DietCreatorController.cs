using FitAppka.Models;
using FitAppka.Service.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FitAppka.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class DietCreatorController : Controller
    {
        private readonly IDietCreatorService _dietCreatorSerivce;
        public DietCreatorController(IDietCreatorService dietCreatorSerivce)
        {
            _dietCreatorSerivce = dietCreatorSerivce;
        }

        [HttpGet]
        public IActionResult ActiveDiets(int dayOfWeek)
        {
            return View(_dietCreatorSerivce.GetActiveDiet(dayOfWeek));
        }

        [HttpGet]
        public IActionResult CreateDiet(List<DietProductDTO> addedProducts, string search, bool searched)
        {
            ViewData["wasSearched"] = searched;
            return View(_dietCreatorSerivce.SearchProducts(addedProducts, search));
        }

        [HttpPost]
        public IActionResult CreateDiet(List<DietProductDTO> addedProducts, int productId, int grammage)
        {
            return View(_dietCreatorSerivce.AddProduct(addedProducts, productId, grammage));
        }
    }
}
