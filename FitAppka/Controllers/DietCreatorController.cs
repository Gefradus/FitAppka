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
        private readonly IDietCreatorService _dietCreatorSerivce;
        public DietCreatorController(IDietCreatorService dietCreatorSerivce, IDayManageService dayManageService)
        {
            _dayManageService = dayManageService;
            _dietCreatorSerivce = dietCreatorSerivce;
        }

        [HttpGet]
        public IActionResult ActiveDiets(int dayOfWeek)
        {
            ViewData["dayID"] = _dayManageService.GetTodayId();
            ViewData["dayOfWeek"] = dayOfWeek;
            return View(_dietCreatorSerivce.GetActiveDiet(dayOfWeek));
        }

        [HttpGet]
        public IActionResult CreateDiet(string search, bool searched)
        {
            ViewData["dayID"] = _dayManageService.GetTodayId();
            return View(_dietCreatorSerivce.SearchProducts(search, searched));
        }

        [HttpPost]
        public JsonResult CreateDiet(List<DietProductDTO> products, DietDTO dietDTO, bool overriding)
        {
            return Json(_dietCreatorSerivce.CreateDiet(products, dietDTO, overriding));
        }

        [HttpPost]
        public JsonResult AddProduct(List<DietProductDTO> addedProducts, int productId, int grammage)
        {
            return Json(_dietCreatorSerivce.AddProduct(addedProducts, productId, grammage, false));
        }

        [HttpDelete]
        public JsonResult DeleteProduct(List<DietProductDTO> addedProducts, int tempId)
        {
            return Json(_dietCreatorSerivce.DeleteProduct(addedProducts, tempId));
        }

    }
}
