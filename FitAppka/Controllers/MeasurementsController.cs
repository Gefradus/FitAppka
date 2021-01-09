using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FitnessApp.Service;
using FitnessApp.Repository;
using FitnessApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FitnessApp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class MeasurementsController : Controller
    {
        private readonly IDayManageService _dayService;
        private readonly FitAppContext _context;
        private readonly IClientRepository _clientRepository;
        private readonly IMeasurementsService _measurementsService;

        public MeasurementsController(IDayManageService dayService, FitAppContext context, 
            IClientRepository clientRepository, IMeasurementsService measurementsService)
        {
            _measurementsService = measurementsService;
            _context = context;
            _clientRepository = clientRepository;
            _dayService = dayService;
        }

        [HttpGet]
        [Route("/Measurements")]
        public async Task<IActionResult> Measurements()
        {
            ViewData["dayID"] = _dayService.GetTodayId();
            return View(await _context.WeightMeasurement.Where(w => w.ClientId == _clientRepository.GetLoggedInClientId()).Include(w => w.FatMeasurement).ToListAsync());
        }

        [HttpPost]
        [Route("/Measurements")]
        public JsonResult Measurements(double weight, int waist) 
        {
            _measurementsService.AddMeasurements(weight, waist);
            return Json(true);
        }

        [HttpPut]
        [Route("/Measurements")]
        public JsonResult Measurements(int id, double weight, int waist)
        {
            return Json(_measurementsService.UpdateMeasurements(id, weight, waist));
        }

        [HttpDelete]
        [Route("/Measurements")]
        public JsonResult Measurements(int id)
        {
            return Json(_measurementsService.DeleteMeasurement(id));
        }
    }
}
