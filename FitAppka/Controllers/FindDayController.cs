using FitnessApp.Models;
using FitnessApp.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class FindDayController : Controller 
    { 
    
        private readonly IFindDayService _findDayService;
        
        public FindDayController(IFindDayService findDayService) {
            _findDayService = findDayService;
        }

        [HttpGet]
        [Route("/FindDay")]
        public IActionResult FindDay(FindDayDTO dto)
        {
            return View(_findDayService.FindDays(dto));
        }

    }
}