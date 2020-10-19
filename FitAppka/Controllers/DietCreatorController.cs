using FitAppka.Models;
using FitAppka.Service;
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
        public IActionResult ActiveDiets(int dayOfWeek, bool wasDayChanged)
        {
            ViewData["dayID"] = _dayManageService.GetTodayId();
            return View(_dietCreatorSerivce.GetActiveDiet(dayOfWeek, wasDayChanged));
        }

        [HttpGet]
        public IActionResult CreateDiet(string search, bool searched)
        {
            ViewData["dayID"] = _dayManageService.GetTodayId();
            return View(_dietCreatorSerivce.SearchProducts(search, searched));
        }

        [HttpGet]
        public IActionResult MyDiets() {
            ViewData["dayID"] = _dayManageService.GetTodayId();
            return View(_dietCreatorSerivce.GetActiveDiets());
        }

        [HttpGet]
        public IActionResult EditDiet(int id, string search, bool searched, bool change)
        {
            ViewData["change"] = change;
            ViewData["dayID"] = _dayManageService.GetTodayId();
            return View(_dietCreatorSerivce.EditDietSearchProduct(id, search, searched));
        }

        [HttpPost]
        public JsonResult EditDiet(List<DietProductDTO> products, DietDTO dietDTO, bool overriding)
        {
            return Json(_dietCreatorSerivce.EditDiet(products, dietDTO, overriding));
        }

        [HttpPost]
        public JsonResult CreateDiet(List<DietProductDTO> products, DietDTO dietDTO, bool overriding)
        {
            return Json(_dietCreatorSerivce.CreateDiet(products, dietDTO, overriding));
        }

        [HttpPost]
        public JsonResult AddProduct(List<DietProductDTO> addedProducts, int productId, int grammage)
        {
            ViewData["change"] = true;
            return Json(_dietCreatorSerivce.AddProduct(addedProducts, productId, grammage, false));
        }

        [HttpDelete]
        public JsonResult DeleteProduct(List<DietProductDTO> addedProducts, int tempId)
        {
            ViewData["change"] = true;
            return Json(_dietCreatorSerivce.DeleteProduct(addedProducts, tempId));
        }

        [HttpDelete]
        public JsonResult DeleteDiet(int id)
        {
            return Json(_dietCreatorSerivce.DeleteDiet(id));
        }

    }
}
