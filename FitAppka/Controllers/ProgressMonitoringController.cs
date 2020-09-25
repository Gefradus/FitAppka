using Microsoft.AspNetCore.Mvc;
using FitAppka.Service;

namespace FitAppka.Controllers
{
    public class ProgressMonitoringController : Controller
    {
        private readonly IDayManageService _dayService;
        private readonly IProgressMonitoringService _progressMonitoringService;

        public ProgressMonitoringController(IProgressMonitoringService progressMonitoringService,
            IDayManageService dayService)
        {
            _progressMonitoringService = progressMonitoringService;
            _dayService = dayService;
        }

        [HttpGet]
        public IActionResult ProgressMonitoring(string dateFrom, string dateTo)
        {
            ViewData["dayID"] = _dayService.GetTodayId();
            ViewData["weightMeasurements"] = _progressMonitoringService.GetWeightMeasurementListFromTo(dateFrom, dateTo);
            ViewData["dateFrom"] = _progressMonitoringService.ConvertToJSDate(dateFrom, true);
            ViewData["dateTo"] = _progressMonitoringService.ConvertToJSDate(dateTo, false);
            return View();
        }



    }
}
