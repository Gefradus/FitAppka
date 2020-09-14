using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FitAppka.Controllers
{
    public class ProgressMonitoringController : Controller
    {

        [HttpGet]
        public IActionResult ProgressMonitoring()
        {
            return View();
        }
    }
}
