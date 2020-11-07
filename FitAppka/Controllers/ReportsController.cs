using FitAppka.Reports;
using FitAppka.Repository;
using FitAppka.Service.ServiceInterface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FitAppka.Controllers
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

        public ActionResult GenerateDaysReport()
        {
            DaysReport dayReport = new DaysReport(_hostEnvironment);
            return File(dayReport.Report(_reportService.GetDaysSummary()), "application/pdf");
        }

        public ActionResult GenerateDaySummaryReport(int id)
        {
            DaySummaryReport daySummaryReport = new DaySummaryReport(_hostEnvironment);
            return File(daySummaryReport.Report(_reportService.GetDayWithDetails(id)), "application/pdf");
        }
    }
}
