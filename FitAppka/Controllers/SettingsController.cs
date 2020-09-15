using System.Threading.Tasks;
using FitAppka.Model;
using FitAppka.Repository;
using FitAppka.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitAppka.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly IClientRepository _clientRepository;
        private readonly ISettingsService _settingsService;
        private readonly IDietaryTargetsService _dietaryTargetsService;

        public SettingsController(IClientRepository clientRepository, ISettingsService settingsService, IDietaryTargetsService dietaryTargetsService)
        {
            _dietaryTargetsService = dietaryTargetsService;
            _clientRepository = clientRepository;
            _settingsService = settingsService;
        }

        [HttpGet]
        public IActionResult Settings()
        {
            FirstAppLaunch();
            ViewData["clientID"] = _clientRepository.GetLoggedInClient().ClientId;
            return View();
        }

        private void FirstAppLaunch()
        {
            try
            {
                Client client = _clientRepository.GetLoggedInClient();
                ViewData["dateOfBirth"] = client.DateOfBirth.Value.ToString("yyyy-MM-dd");
                ViewData["weight"] = _dietaryTargetsService.GetLastWeightMeasurement();
                ViewData["growth"] = client.Growth;
                ViewData["changeGoal"] = (int)client.WeightChangeGoal;
                ViewData["activity"] = (int)client.ActivityLevel;
                ViewData["pace"] = client.PaceOfChanges.ToString().Replace(',', '.');
                ViewData["isFirstLaunch"] = 0;
                SendDataAboutClientBooleans(client);
            }
            catch
            {
                ViewData["dateOfBirth"] = "2000-01-01";
                ViewData["isFirstLaunch"] = 1;
            }
        }

        public void SendDataAboutClientBooleans(Client client)
        {
            SetBoolean(client.Sex, "sex");
            SetBoolean(client.Breakfast, "breakfast");
            SetBoolean(client.Lunch, "lunch");
            SetBoolean(client.Dinner, "dinner");
            SetBoolean(client.Dessert, "dessert");
            SetBoolean(client.Snack, "snack");
            SetBoolean(client.Supper, "supper");
        }

        private void SetBoolean(bool? flag, string viewDataName)
        {
            if ((bool)flag) { 
                ViewData[viewDataName] = 1;
            }
            else {
                ViewData[viewDataName] = 0;
            }
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Settings(SettingsModel m, int isFirstLaunch)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _settingsService.ChangeSettings(m, isFirstLaunch);
                    return RedirectToAction("Start", "Home");
                }
                catch
                {
                    ModelState.AddModelError("", "Data urodzenia nie może być mniejsza niż 01.01.1900r.");
                    return View(m);
                }
            }

            FirstAppLaunch();
            return View(m);
        }

    }
}

