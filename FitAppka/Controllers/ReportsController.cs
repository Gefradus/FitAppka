using FitAppka.Reports;
using FitAppka.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FitAppka.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IDayRepository _dayRepository;
        private readonly IClientRepository _clientRepository;

        public ReportsController(IDayRepository dayRepository, IWebHostEnvironment hostEnvironment, IClientRepository clientRepository)
        {
            _hostEnvironment = hostEnvironment;
            _clientRepository = clientRepository;
            _dayRepository = dayRepository;
        }

        [HttpGet]
        public IActionResult ExportReport()
        {
            return View();
        }

        public ActionResult GenerateDaysReport()
        {
            DayReport dayReport = new DayReport(_hostEnvironment);
            return File(dayReport.Report(_dayRepository.GetClientDays(_clientRepository.GetLoggedInClientId()).ToList()), "application/pdf");
        }
    }
}
