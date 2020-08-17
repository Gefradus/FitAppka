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
        public async Task<IActionResult> Logowanie(Klient model)
        {
            if (ModelState.IsValid)
            {
                int klientID = model.KlientId;
                string loginOrEmail = model.Login.ToLower();
                string haslo = model.Haslo;

                Klient klient = _context.Klient.Where(k => k.Login.ToLower().Equals(loginOrEmail) || k.Email.ToLower().Equals(loginOrEmail)).FirstOrDefault();
                if (klient != null && klient.Haslo.Equals(haslo))
                {
                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, klient.Login) };

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
                var klientPoEmail = _context.Klient.Where(k => k.Email.ToLower() == model.Email.ToLower()).FirstOrDefault();
                var klientPoLogin = _context.Klient.Where(k => k.Login.ToLower() == model.Login.ToLower()).FirstOrDefault();

                if (klientPoEmail == null && klientPoLogin == null)
                {
                    if (model.ConfirmPassword == model.Password)
                    {

                        var nowyKlient = new Klient()
                        {
                            Login = model.Login,
                            Email = model.Email,
                            Haslo = model.Password,
                            Imie = model.FirstName,
                            Nazwisko = model.SecondName
                        };

                        _context.Add(nowyKlient);
                        await _context.SaveChangesAsync();

                        var claims = new List<Claim> { new Claim(ClaimTypes.Name, model.Login) };

                        var userIdentity = new ClaimsIdentity(claims, "login");


                        ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                        await HttpContext.SignInAsync(principal);
                        int id = _context.Klient.Where(k => k.Login.ToLower() == model.Login.ToLower()).Select(k => k.KlientId).FirstOrDefault();

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
            

