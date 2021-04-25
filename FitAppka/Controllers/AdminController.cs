using AutoMapper;
using FitnessApp.Models;
using FitnessApp.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitnessApp.Service.ServiceInterface;

namespace FitnessApp.Controllers
{

    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IProductManageService _productManageService;
        private readonly IAdministrationService _administrationService;
        private readonly IClientManageService _clientManageService;
        private readonly IMapper _mapper;

        public AdminController(IClientManageService clientManageService, IProductManageService productManageService, 
            IMapper mapper, IAdministrationService administrationService)
        {
            _administrationService = administrationService;
            _mapper = mapper;
            _productManageService = productManageService;
            _clientManageService = clientManageService;
        }

        [HttpGet]
        public IActionResult AdminHome()
        {
            if (_clientManageService.IsLoggedInClientAdmin()) {
                return View();
            }
            else {
                return RedirectToAction("Logout", "Login");
            }
        }

        [HttpGet]
        public IActionResult AdminTraining(string searchCardio, string searchStrength)
        {
            if (_clientManageService.IsLoggedInClientAdmin()) {
                return View(_administrationService.GetTrainingsDTO(searchCardio, searchStrength));
            }
            else {
                return RedirectToAction("Logout", "Login");
            }
        }

      

        [HttpGet]
        public IActionResult AdminProduct(string search, int? page)
        {
            if (_clientManageService.IsLoggedInClientAdmin()) {
                return View(_productManageService.AdminDto(search, false, 0, false, page));
            }
            else {
                return RedirectToAction("Logout", "Login");
            }
        }

        [HttpGet]
        public IActionResult AdminEditProduct(int id)
        {
            if (_clientManageService.IsLoggedInClientAdmin())
            {
                return View(SendDataAboutProductToView(id));
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
        }

        [HttpGet]
        public IActionResult AdminDiet()
        {
            if (_clientManageService.IsLoggedInClientAdmin()) 
            {
                return View(_administrationService.GetAdminDiets());
            } 
            else
            {
                return RedirectToAction("Logout", "Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdminEditProduct(ProductDTO model, int productID, int addOrEditPhoto)
        {
            if (ModelState.IsValid)
            {
                _productManageService.UpdateProduct(model, productID, addOrEditPhoto);
                return RedirectToAction(nameof(AdminProduct));
            }
            return View(model);
        }

        private ProductDTO SendDataAboutProductToView(int id)
        {
            Product product = _productManageService.GetProduct(id);
            
            if (product.PhotoPath == null) {
                ViewData["addOrEdit"] = 0;
            }
            else {
                ViewData["addOrEdit"] = 1;
            }

            return _mapper.Map<Product, ProductDTO>(product);
        }

        [HttpPost]
        public JsonResult DeleteProduct(int productID)
        {
            if (_clientManageService.IsLoggedInClientAdmin())
            {
                _productManageService.Delete(productID);
                return Json(true);
            }
            return Json(false);
        }


        [HttpGet]
        public IActionResult AdminClient()
        {
            if (_clientManageService.IsLoggedInClientAdmin()) {
                return View(_clientManageService.GetAllClientsAndSortByAdminAndBanned());
            }
            else {
                return RedirectToAction("Logout", "Login");
            }
        }

        [HttpGet]
        public IActionResult AdminEditClient(int id)
        {
            if (_clientManageService.IsLoggedInClientAdmin()) {
                ViewData["id"] = id;
                return View(_administrationService.GetClientAdministrationDTO(id));
            }
            else {
                return RedirectToAction("Logout", "Login");
            }
        }

        [HttpPost]
        public IActionResult AdminEditClient(ClientAdministrationDTO dto, int id)
        {
            _administrationService.EditClient(dto, id);
            return RedirectToAction(nameof(AdminEditClient));
        }

        [HttpPost]
        public JsonResult CardioType(string name, int kcalPerMin, bool visibleToAll)
        {
           return Json(_administrationService.AddCardioType(name, kcalPerMin, visibleToAll));
        }

        [HttpPut]
        public JsonResult CardioType(int id, string name, int kcalPerMin, bool visibleToAll) {
            return Json(_administrationService.EditCardioType(id, name, kcalPerMin, visibleToAll));
        }

        [HttpDelete]
        public JsonResult CardioType(int id)
        {
            return Json(_administrationService.DeleteCardioType(id));
        }

        [HttpPost]
        public JsonResult StrengthTrainingType(string name, bool visibleToAll)
        {
            return Json(_administrationService.AddStrengthTrainingType(name, visibleToAll));
        }

        [HttpPut]
        public JsonResult StrengthTrainingType(int id, string name, bool visibleToAll)
        {
            return Json(_administrationService.EditStrengthTrainingType(id, name, visibleToAll));
        }

        [HttpDelete]
        public JsonResult StrengthTrainingType(int id)
        {
            return Json(_administrationService.DeleteStrengthTrainingType(id));
        }

        [HttpPost]
        public JsonResult UnbanClient(int id)
        {
            return Json(_administrationService.UnbanClient(id));
        }

        [HttpPost]
        public JsonResult BanClient(int id)
        {
            return Json(_administrationService.BanClient(id));
        }
 
    }
}
