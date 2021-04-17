using Microsoft.AspNetCore.Mvc;
using FitnessApp.Service;
using Microsoft.AspNetCore.Authorization;

namespace FitnessApp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class MeasurementsController : Controller
    {
        private readonly IDayManageService _dayService;
        private readonly IMeasurementsService _measurementsService;

        public MeasurementsController(IDayManageService dayService, IMeasurementsService measurementsService)
        {
            _measurementsService = measurementsService;
            _dayService = dayService;
        }

        [HttpGet]
        [Route("/Measurements")]
        public IActionResult Measurements()
        {
            ViewData["dayID"] = _dayService.GetTodayId();
            return View(_measurementsService.Dto());
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
