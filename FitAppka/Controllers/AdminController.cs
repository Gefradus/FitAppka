using System.Threading.Tasks;
using AutoMapper;
using FitAppka.Models;
using FitAppka.Repository;
using FitAppka.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace FitAppka.Controllers
{

    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IProductManageService _productManageService;
        private readonly IClientRepository _clientRepository;
        private readonly FitAppContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public AdminController(FitAppContext context, IWebHostEnvironment env, IClientRepository clientRepository, 
            IProductManageService productManageService, IMapper mapper)
        {
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
        public IActionResult AdminSettings()
        {
            if (_clientRepository.IsLoggedInClientAdmin()) {
                return View();
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
        public JsonResult Delete(int productID)
        {
            if (_clientRepository.IsLoggedInClientAdmin())
            {
                _productManageService.Delete(productID);
                return Json(true);
            }
            return Json(false);
        }


        [HttpGet]
        public async Task<IActionResult> AdminClient()
        {
            if (_clientRepository.IsLoggedInClientAdmin())
            {
                return View(await _clientRepository.GetAllClientsAsync());
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
        }


        
    }
}
