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
            MonthSummaryReport dayReport = new MonthSummaryReport(_hostEnvironment);
            return File(dayReport.Report(_reportService.GetMonthSummary(id)), "application/pdf");
        }

        public ActionResult GenerateDaySummaryReport(int id)
        {
            DaySummaryReport daySummaryReport = new DaySummaryReport(_hostEnvironment);
            return File(daySummaryReport.Report(_reportService.GetDayWithDetails(id)), "application/pdf");
        }
    }
}
