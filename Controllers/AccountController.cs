using AzureCustomerOPeration.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace AzureCustomerOPeration.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();     
        }
        [HttpPost]  
        public async Task<IActionResult> Login(LoginModel model)
        {
        if (LoginUser(model.Username, model.Password)){
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username)
                };
                var userIdentity = new ClaimsIdentity(claims, "login");
                await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(userIdentity));
                return RedirectToAction("Index", "CustomerDetails");
            }
            return View();
        }

        private bool LoginUser(object username, object password)
        {
           return true;
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Unauthorized()
        {
            return View();
        }
    }
}
