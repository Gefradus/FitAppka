using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitnessApp.Models;
using FitnessApp.Service;

namespace FitnessApp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class GoalsController : Controller
    {
        private readonly IGoalsService _goalsService;

        public GoalsController(IGoalsService goalsService) { 
            _goalsService = goalsService;
        }

        [HttpGet]
        [Route("/Goals")]
        public IActionResult Goals()
        {
            return View(_goalsService.MapClientGoalsToCreateGoalsModel());
        }

        [HttpPost]
        [Route("/Goals")]
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
