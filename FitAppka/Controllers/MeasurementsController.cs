using Microsoft.AspNetCore.Mvc;
using FitnessApp.Service;
using Microsoft.AspNetCore.Authorization;

namespace FitnessApp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class MeasurementsController : Controller
    {
        private readonly IMeasurementsService _measurementsService;

        public MeasurementsController(IMeasurementsService measurementsService)
        {
            _measurementsService = measurementsService;
        }

        [HttpGet]
        [Route("/Measurements")]
        public IActionResult Measurements(int? page)
        {
            return View(_measurementsService.Dto(page));
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
