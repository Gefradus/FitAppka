using System.Threading.Tasks;
using AutoMapper;
using FitAppka.Models;
using FitAppka.Repository;
using FitAppka.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using FitAppka.Service.ServiceInterface;

namespace FitAppka.Controllers
{

    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IProductManageService _productManageService;
        private readonly IAdministrationService _administrationService;
        private readonly IWeightMeasurementRepository _weightMeasurementRepository;
        private readonly IDayManageService _dayManageService;
        private readonly IClientRepository _clientRepository;
        private readonly FitAppContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public AdminController(FitAppContext context, IWebHostEnvironment env, IClientRepository clientRepository, 
            IProductManageService productManageService, IMapper mapper, IAdministrationService administrationService,
            IWeightMeasurementRepository weightMeasurementRepository, IDayManageService dayManageService)
        {
            _dayManageService = dayManageService;
            _weightMeasurementRepository = weightMeasurementRepository;
            _administrationService = administrationService;
            _mapper = mapper;
            _productManageService = productManageService;
            _clientRepository = clientRepository;
            _context = context;
            _env = env;
        }

        [HttpGet]
        public IActionResult AdminHome()
        {
            if (_clientRepository.IsLoggedInClientAdmin()) {
                return View();
            }
            else {
                return RedirectToAction("Logout", "Login");
            }
        }

        [HttpGet]
        public IActionResult AdminTraining(string searchCardio, string searchStrength)
        {
            if (_clientRepository.IsLoggedInClientAdmin()) {
                return View(_administrationService.GetTrainingsDTO(searchCardio, searchStrength));
            }
            else {
                return RedirectToAction("Logout", "Login");
            }
        }

      

        [HttpGet]
        public async Task<IActionResult> AdminProduct(string search)
        {
            if (_clientRepository.IsLoggedInClientAdmin()) {
                return View(await _productManageService.SearchProduct(search, false));
            }
            else {
                return RedirectToAction("Logout", "Login");
            }
        }

        [HttpGet]
        public IActionResult AdminEditProduct(int id)
        {
            if (_clientRepository.IsLoggedInClientAdmin())
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
            if (_clientRepository.IsLoggedInClientAdmin()) 
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
            if (_clientRepository.IsLoggedInClientAdmin())
            {
                _productManageService.Delete(productID);
                return Json(true);
            }
            return Json(false);
        }


        [HttpGet]
        public IActionResult AdminClient()
        {
            if (_clientRepository.IsLoggedInClientAdmin()) {
                return View(_clientRepository.GetAllClientsAndSortByAdminAndBanned());
            }
            else {
                return RedirectToAction("Logout", "Login");
            }
        }

        [HttpGet]
        public IActionResult AdminEditClient(int id)
        {
            if (_clientRepository.IsLoggedInClientAdmin()) {
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
