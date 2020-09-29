using FitAppka.Service.ServiceImpl;
using Microsoft.AspNetCore.Mvc;

namespace FitAppka.Controllers
{
    public class ContentRootController : Controller
    {
        private readonly IContentRootPathHandlerService _service;

        public ContentRootController(IContentRootPathHandlerService service)
        {
            _service = service;
        }

        [HttpGet]
        public JsonResult Get() {
            return Json(_service.GetContentRootFileName());
        }
    }
}
