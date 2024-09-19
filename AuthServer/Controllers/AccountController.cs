using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AuthServer.Models;

namespace AuthServer.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthDbContext _authDbContext;

        public AccountController(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }
        /// <summary>
        /// Kimlik doğrulamasının yapılacağı yer. 
        /// Şimdilik doğrumuş gibi kabul edip devam ediyoruz.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = _authDbContext.Set<User>().Where(x => x.Password == model.Password && x.UserName == model.UserName).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    List<Claim> claims = new() { new(ClaimTypes.Name, user.Name) };
                    ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new(identity);
                    await HttpContext.SignInAsync(principal);

                    if (Url.IsLocalUrl(TempData["returnUrl"]?.ToString()))
                        return Redirect(TempData["returnUrl"].ToString());

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(model);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
