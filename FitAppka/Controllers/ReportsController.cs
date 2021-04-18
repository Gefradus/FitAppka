using FitnessApp.Reports;
using FitnessApp.Service.ServiceInterface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IReportService _reportService;

        public ReportsController(IWebHostEnvironment hostEnvironment, IReportService reportService)
        {
            _hostEnvironment = hostEnvironment;
            _reportService = reportService;
        }

        public ActionResult GenerateMonthSummaryReport(int id)
        {
            return File(new MonthSummaryReport(_hostEnvironment).Report(_reportService.GetMonthSummary(id)), "application/pdf");
        }

        public ActionResult GenerateDaySummaryReport(int id)
        {
            return File(new DaySummaryReport(_hostEnvironment).Report(_reportService.GetDayWithDetails(id)), "application/pdf");
        }
    }
}
