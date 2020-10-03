using FitAppka.Service.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Route("/DietCreator")]
        public IActionResult ActiveDiets(int dayOfWeek)
        {
            return View(_dietCreatorSerivce.GetActiveDiet(dayOfWeek));
        }
    }
}
