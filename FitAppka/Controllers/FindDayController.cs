using FitAppka.Repository;
using FitAppka.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitAppka.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class FindDayController : Controller
    {
        private readonly IFindDayService _findDayService;
        private readonly IDayManageService _dayManageService;
        private readonly IClientRepository _clientRepository;
        
        public FindDayController(IClientRepository clientRepository, IDayManageService dayManageService, IFindDayService findDayService)
        {
            _findDayService = findDayService;
            _dayManageService = dayManageService;
            _clientRepository = clientRepository;
        }

        [HttpGet]
        public IActionResult FindDay(int productID, int from, int to, int searchType)
        {
            var clientID = _clientRepository.GetLoggedInClientId();
            var listOfDays = _findDayService.FindDays(from, to, productID, searchType, clientID);

            ViewData["from"] = from;
            ViewData["to"] = to;
            ViewData["searchType"] = searchType;
            ViewData["daysID"] = listOfDays;
            ViewData["clientID"] = clientID;
            ViewData["productID"] = productID;
            ViewData["dayID"] = _dayManageService.GetTodayId();
            ViewData["allProducts"] = _findDayService.CreateProductsList(productID);
            ViewData["wheterFound"] = _findDayService.WheterDayFound(listOfDays);
            ViewData["listByWater"] = _findDayService.WaterInDays(listOfDays);
            ViewData["listByKcal"] = _findDayService.CaloriesInDays(listOfDays);
            ViewData["listByGrammages"] = _findDayService.GrammageInDays(listOfDays, productID);
                         
            return View(_dayManageService.GetAllDays());
        }

    }
}