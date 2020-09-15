using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FitAppka.Service;
using FitAppka.Repository;
using FitAppka.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;



namespace FitAppka.Controllers
{
    public class MeasurementsController : Controller
    {
        private IDayManageService _dayService;
        private IWeightMeasurementRepository _weightMeasurementRepository;
        private FitAppContext _context;
        private IClientRepository _clientRepository;

        public MeasurementsController(IDayManageService dayService, FitAppContext context, IClientRepository clientRepository)
        {
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
    }
}
