using FitAppka.Models;
using FitAppka.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitAppka.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class FindDayController : Controller
    {
        private readonly IDayManageService _dayManageService;
        
        public FindDayController(IDayManageService dayManageService)
        {
            _dayManageService = dayManageService;
        }

        [HttpGet]
        public IActionResult FindDay(FindDayDTO findDayDTO)
        {
            ViewData["dayID"] = _dayManageService.GetTodayId();
            return View("FindDay2");
        }

    }
}