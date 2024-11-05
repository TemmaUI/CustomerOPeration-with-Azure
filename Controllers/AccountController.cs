using AzureCustomerOPeration.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;

namespace AzureCustomerOPeration.Controllers
{
    public class AccountController : Controller
    {
        public List<UserModel> users = null;
        public AccountController()
        {
            users = new List<UserModel>();
            users.Add(new UserModel()
            {
                UserId = 1,
                UserName = "Temmaui",
                Password = "123",
                Role = "Admin"
            });
            users.Add(new UserModel()
            {
                UserId = 2,
                UserName = "user",
                Password = "123",
                Role = "User"
            });
        }
        public IActionResult Login(string returnUrl="/")
        {
            LoginModel loginModel = new LoginModel();   
            loginModel.ReturnUrl = returnUrl;
            return View(loginModel);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)

        {
            var user = users.Where(u => u.UserName == loginModel.UserName && u.Password == loginModel.Password).FirstOrDefault();
            if (user != null)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.UserId)),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Name, user.Role),
                    new Claim("AzureCustomerOPeration", "Code"),

                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                    new AuthenticationProperties()
                    {
                        IsPersistent = loginModel.RememberLogin
                    });
                return LocalRedirect(loginModel.ReturnUrl);
            }
            else
            {
                ViewBag.Message = "Invalid Credentials";
                return View(loginModel);
            }
        }
                public async Task<IActionResult> Logout()
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/");
        }
    }
}

