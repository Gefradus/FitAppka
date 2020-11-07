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
            return File(dayReport.Report(_reportService.GetDays()), "application/pdf");
        }

        /*public ActionResult GenerateDaySummaryReport()
        {

        }*/
    }
}
