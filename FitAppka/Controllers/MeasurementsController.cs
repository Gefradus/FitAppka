using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FitAppka.Service;
using FitAppka.Repository;
using FitAppka.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace FitAppka.Controllers
{
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
        public async Task<IActionResult> Measurements()
        {
            ViewData["dayID"] = _dayService.GetTodayId();
            return View(await _context.WeightMeasurement.Where(w => w.ClientId == _clientRepository.GetLoggedInClientId()).Include(w => w.FatMeasurement).ToListAsync());
        }

        [HttpPost]
        public JsonResult Measurements(short weight, int waist)
        {
            return Json(_measurementsService.AddOrUpdateMeasurements(weight, waist));
        }


        [HttpDelete]
        public JsonResult Measurements(int id)
        {
            return Json(_measurementsService.DeleteMeasurement(id));
        }
    }
}
