using Microsoft.AspNetCore.Mvc;
using FitAppka.Service;

namespace FitAppka.Controllers
{
    public class ProgressMonitoringController : Controller
    {
        private readonly IDayManageService _dayService;

        public ProgressMonitoringController(IDayManageService dayService)
        {
            _dayService = dayService;
        }

        [HttpGet]
        public IActionResult ProgressMonitoring()
        {
            ViewData["dayID"] = _dayService.GetTodayId();
            return View();
        }
    }
}
