using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FitAppka.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace FitAppka.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class LoginController : Controller
    {
        private readonly FitAppContext _context;

        public LoginController(FitAppContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Logowanie()
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
            
            return RedirectToAction(nameof(Logowanie));
        }

        [HttpPost]
        public async Task<IActionResult> Logowanie(Client model)
        {
            if (ModelState.IsValid)
            {
                int clientID = model.ClientId;
                string loginOrEmail = model.Login.ToLower();
                string pass = model.Password;

                Client client = _context.Client.Where(c => c.Login.ToLower().Equals(loginOrEmail) || c.Email.ToLower().Equals(loginOrEmail)).FirstOrDefault();
                if (client != null && client.Password.Equals(pass))
                {
                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, client.Login) };

                    ClaimsPrincipal principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "login"));
                    await HttpContext.SignInAsync(principal);

                    return RedirectToAction("Start", "Posilek");
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
                var klientPoEmail = _context.Client.Where(k => k.Email.ToLower() == model.Email.ToLower()).FirstOrDefault();
                var klientPoLogin = _context.Client.Where(k => k.Login.ToLower() == model.Login.ToLower()).FirstOrDefault();

                if (klientPoEmail == null && klientPoLogin == null)
                {
                    if (model.ConfirmPassword == model.Password)
                    {

                        var nowyKlient = new Client()
                        {
                            Login = model.Login,
                            Email = model.Email,
                            Password = model.Password,
                            FirstName = model.FirstName,
                            SecondName = model.SecondName
                        };

                        _context.Add(nowyKlient);
                        await _context.SaveChangesAsync();

                        var claims = new List<Claim> { new Claim(ClaimTypes.Name, model.Login) };

                        var userIdentity = new ClaimsIdentity(claims, "login");


                        ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                        await HttpContext.SignInAsync(principal);
                        int id = _context.Client.Where(k => k.Login.ToLower() == model.Login.ToLower()).Select(k => k.ClientId).FirstOrDefault();

                        return RedirectToAction("Start", "Posilek", new { id });
                    }
                }
                else
                {
                    if (klientPoEmail != null)
                    {
                        ModelState.AddModelError(string.Empty, "Istnieje już użytkownik o podanym adresie e-mail");
                    }

                    if(klientPoLogin != null)
                    {
                        ModelState.AddModelError(string.Empty, "Istnieje już użytkownik o podanym loginie");
                    }
                }

                return View(model);
            }

            return View(model);
        }


    }
       
}
            

