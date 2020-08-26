using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FitAppka.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using FitAppka.Repository;

namespace FitAppka.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class LoginController : Controller
    {
        private readonly FitAppContext _context;
        private readonly IClientRepository _clientRepository;

        public LoginController(FitAppContext context, IClientRepository clientRepository)
        {
            _context = context;
            _clientRepository = clientRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public async Task<IActionResult> Login(Client model)
        {
            if (ModelState.IsValid)
            {
                string loginOrEmail = model.Login.ToLower();
                Client client = _context.Client.Where(c => c.Login.ToLower().Equals(loginOrEmail) || c.Email.ToLower().Equals(loginOrEmail)).FirstOrDefault();
                if (client != null && client.Password.Equals(model.Password))
                {
                    await SignInAndStart(client.Login);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Nieprawidłowy login i/lub hasło");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var clientByEmail = _clientRepository.GetClientByEmail(model.Email);
                var clientByLogin = _clientRepository.GetClientByLogin(model.Login);

                if (clientByEmail == null && clientByLogin == null)
                {
                    if (model.ConfirmPassword == model.Password)
                    {
                        await AddNewClientAndLogin(model);
                    }
                }
                else
                {
                    if (clientByEmail != null) {
                        ModelState.AddModelError(string.Empty, "Istnieje już użytkownik o podanym adresie e-mail");
                    }
                    if (clientByLogin != null) {
                        ModelState.AddModelError(string.Empty, "Istnieje już użytkownik o podanym loginie");
                    }
                }

                return View(model);
            }

            return View(model);
        }


        private async Task AddNewClientAndLogin(RegisterModel model)
        {
            await _clientRepository.AddAsync(new Client()
            {
                Login = model.Login,
                Email = model.Email,
                Password = model.Password,
                FirstName = model.FirstName,
                SecondName = model.SecondName
            });

            await SignInAndStart(model.Login);
        }


        private async Task<IActionResult> SignInAndStart(string login)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, login) };
            await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "login")));
            return RedirectToAction("Start", "Home");
        }
    }  
}
            

