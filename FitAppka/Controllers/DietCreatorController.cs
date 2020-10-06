using FitAppka.Models;
using FitAppka.Service;
using FitAppka.Service.ServiceImpl;
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
        private readonly IDayManageService _dayManageService;
        private readonly IContentRootPathHandlerService _contentRootService;
        private readonly IDietCreatorService _dietCreatorSerivce;
        public DietCreatorController(IDietCreatorService dietCreatorSerivce, IContentRootPathHandlerService contentRootService, IDayManageService dayManageService)
        {
            _dayManageService = dayManageService;
            _contentRootService = contentRootService;
            _dietCreatorSerivce = dietCreatorSerivce;
        }

        [HttpGet]
        public IActionResult ActiveDiets(int dayOfWeek)
        {
            ViewData["dayID"] = _dayManageService.GetTodayId();
            return View(_dietCreatorSerivce.GetActiveDiet(dayOfWeek));
        }

        [HttpGet]
        public IActionResult CreateDiet(List<DietProductDTO> addedProducts, string search, bool searched)
        {
            ViewData["dayID"] = _dayManageService.GetTodayId();
            ViewData["path"] = _contentRootService.GetContentRootFileName();
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
