using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FitnessApp.Service;

namespace FitnessApp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class ProgressMonitoringController : Controller
    {
        private readonly IProgressMonitoringService _progressMonitoringService;

        public ProgressMonitoringController(IProgressMonitoringService progressMonitoringService)
        {
            _progressMonitoringService = progressMonitoringService;
        }

        [HttpGet]
        [Route("/ProgressMonitoring")]
        public IActionResult ProgressMonitoring(string dateFrom, string dateTo, short chartType)
        {
            return View(_progressMonitoringService.GetProgressMonitoringDTO(dateFrom, dateTo, chartType));
        }

    }
}
