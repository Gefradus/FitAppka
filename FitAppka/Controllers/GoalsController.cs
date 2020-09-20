using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitAppka.Models;
using FitAppka.Service;

namespace FitAppka.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class GoalsController : Controller
    {
        private readonly IDayManageService _dayService;
        private readonly IGoalsService _goalsService;

        public GoalsController(IDayManageService dayService, IGoalsService goalsService) { 
            _dayService = dayService;
            _goalsService = goalsService;
        }

        [HttpGet]
        public IActionResult Goals()
        {
            ViewData["dayID"] = _dayService.GetTodayId();
            return View(_goalsService.MapClientGoalsToCreateGoalsModel());
        }

        [HttpPost]
        public IActionResult Goals(GoalsDTO model)
        {
            if (ModelState.IsValid) {
                _goalsService.UpdateGoals(model);
                return RedirectToAction("Start", "Home");
            }
            else {
                return View(model);
            }
        }
    }
}
