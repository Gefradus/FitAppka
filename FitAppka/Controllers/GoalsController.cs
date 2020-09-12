using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitAppka.Repository;
using FitAppka.Models;

namespace FitAppka.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class GoalsController : Controller
    {
        private readonly IDayRepository _dayRepository;
        private readonly IClientRepository _clientRepository;
        public GoalsController(IDayRepository dayRepository, IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
            _dayRepository = dayRepository;
        }

        [HttpGet]
        public IActionResult Goals()
        {
            ViewData["dayID"] = _dayRepository.GetClientDays(_clientRepository.GetLoggedInClient().ClientId).Where(d => d.Date == DateTime.Now.Date).FirstOrDefault().DayId;
            return View();
        }

        [HttpPut]
        public IActionResult Goals(CreateGoalsModel model)
        {

            return RedirectToAction("Start", "Home");
        }
    }
}
