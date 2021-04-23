using FitnessApp.Models;
using FitnessApp.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly IClientManageService _clientManageService;
        
        private readonly ISettingsService _settingsService;
        

        public SettingsController(IClientManageService clientManageService, ISettingsService settingsService) {
            _clientManageService = clientManageService;
            _settingsService = settingsService;
        }

        [HttpGet]
        [Route("/Settings")]
        public IActionResult Settings()
        {
            return View(_settingsService.Dto());
        }

        [HttpPost]
        [Route("/Settings")]
        public IActionResult Settings(SettingsDTO m, bool isFirstLaunch)
        {
            if (ModelState.IsValid)
            {
                try {
                    _settingsService.ChangeSettings(m, isFirstLaunch, _clientManageService.GetLoggedInClientId());
                    return RedirectToAction("Start", "Home");
                }
                catch {
                    ModelState.AddModelError("", "Data urodzenia nie może być mniejsza niż 01.01.1900r.");
                    return View(m);
                }
            }

            return View(_settingsService.Dto());
        }
    }
}

