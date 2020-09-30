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
        private readonly IFindDayService _findDayService;
        
        public FindDayController(IDayManageService dayManageService, IFindDayService findDayService)
        {
            _findDayService = findDayService;
            _dayManageService = dayManageService;
        }

        [HttpGet]
        [Route("/FindDay")]
        public IActionResult FindDay(FindDayDTO dto)
        {
            ViewData["dayID"] = _dayManageService.GetTodayId();
            return View(_findDayService.FindDays(dto));
        }

    }
}