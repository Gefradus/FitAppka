using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace FitAppka.Controllers
{
    public class WebRootPathController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public WebRootPathController(IWebHostEnvironment env) {
            _env = env;
        }

        [HttpGet]
        public JsonResult Get() {
            return Json(_env.WebRootPath);
        }
    }
}
