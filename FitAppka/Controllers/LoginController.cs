using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FitnessApp.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using FitnessApp.Service;
using FitnessApp.Models.DTO;

namespace FitnessApp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class LoginController : Controller
    {
        private readonly IClientManageService _clientManageService;

        public LoginController(IClientManageService clientManageService)
        {
            _clientManageService = clientManageService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Login() {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated) {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (model.LoginOrEmail != null)
            {
                if (_clientManageService.CheckIfPassCorrect(model)) {
                    if (_clientManageService.CheckIfClientFromModelIsBanned(model)) {
                        ModelState.AddModelError(string.Empty, "Konto użytkownika za decyzją administracji zostało zablokowane");
                    } 
                    else {
                        return await SignInAndStart(_clientManageService.GetClientLoginFromModel(model));
                    }
                }
                else { ModelState.AddModelError(string.Empty, "Nieprawidłowy login i/lub hasło"); }
            } 
            else { ModelState.AddModelError("LoginOrEmail", "Należy podać login lub e-mail"); }

            return View(model);
        }

        [HttpGet]
        [Route("/Register")]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [Route("/Register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                bool existClientByEmail = _clientManageService.ExistsClientByEmail(model.Email);
                bool existClientByLogin = _clientManageService.ExistsClientByLogin(model.Login);

                if (existClientByEmail && existClientByLogin) {
                    if (model.ConfirmPassword == model.Password) {
                        await _clientManageService.AddNewClient(model);
                        return await SignInAndStart(model.Login);
                    }
                }
                else {
                    if (existClientByEmail) {
                        ModelState.AddModelError(string.Empty, "Istnieje już użytkownik o podanym adresie e-mail");
                    }
                    if (existClientByLogin) {
                        ModelState.AddModelError(string.Empty, "Istnieje już użytkownik o podanym loginie");
                    }
                }

                return View(model);
            }

            return View(model);
        }


        private async Task<IActionResult> SignInAndStart(string login)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, login) };
            await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "login")));
            return RedirectToAction("Start", "Home");
        }
    }  
}
            

