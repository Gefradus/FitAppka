using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FitAppka.Service;

namespace FitAppka.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class ProgressMonitoringController : Controller
    {
        private readonly IDayManageService _dayService;
        private readonly IProgressMonitoringService _progressMonitoringService;

        public ProgressMonitoringController(IProgressMonitoringService progressMonitoringService, IDayManageService dayService)
        {
            _progressMonitoringService = progressMonitoringService;
            _dayService = dayService;
        }

        [HttpGet]
        public IActionResult ProgressMonitoring(string dateFrom, string dateTo, short chartType)
        {
            ViewData["dayID"] = _dayService.GetTodayId();
            return View(_progressMonitoringService.GetProgressMonitoringDTO(dateFrom, dateTo, chartType));
        }

    }
}
