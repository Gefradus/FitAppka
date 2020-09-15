using Microsoft.AspNetCore.Mvc;
using FitAppka.Service;

namespace FitAppka.Controllers
{
    public class MeasurementsController : Controller
    {
        private IDayManageService _dayService;

        public MeasurementsController(IDayManageService dayService)
        {
            _dayService = dayService;
        }

        public IActionResult Measurements()
        {
            ViewData["dayID"] = _dayService.GetTodayId();
            return View();
        }
    }
}
