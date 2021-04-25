using FitnessApp.Models;
using FitnessApp.Service.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FitnessApp.Controllers
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
        public IActionResult ActiveDiets(int dayOfWeek, bool wasDayChanged)
        {
            return View(_dietCreatorSerivce.GetActiveDiet(dayOfWeek, wasDayChanged));
        }

        [HttpGet]
        public IActionResult CreateDiet(string search, bool searched, short isAdmin)
        {
            ViewData["admin"] = isAdmin == 1;
            return View(_dietCreatorSerivce.SearchProducts(search, searched));
        }

        [HttpGet]
        public IActionResult MyDiets(int? page) {
            return View(_dietCreatorSerivce.GetLoggedInClientActiveDiets(page));
        }

        [HttpGet]
        public IActionResult EditDiet(int id, string search, bool searched, bool change, short isAdmin)
        {
            ViewData["admin"] = isAdmin == 1;
            ViewData["change"] = change;
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
            return Json(_dietCreatorSerivce.AddProduct(addedProducts, productId, grammage, false));
        }

        [HttpDelete]
        public JsonResult DeleteProduct(List<DietProductDTO> addedProducts, int tempId)
        {
            return Json(_dietCreatorSerivce.DeleteProduct(addedProducts, tempId));
        }

        [HttpDelete]
        public JsonResult DeleteDiet(int id)
        {
            return Json(_dietCreatorSerivce.DeleteDiet(id));
        }

    }
}
