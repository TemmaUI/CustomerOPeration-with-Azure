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
                Password = "Admin123!",
                Role = "Admin"
            });
            users.Add(new UserModel()
            {
                UserId = 2,
                UserName = "salesrep",
                Password = "SalesRep123!",
                Role = "SalesRep"
            });
        }

        public IActionResult Login(string returnUrl = "/")
        {
            LoginModel loginModel = new LoginModel();
            loginModel.ReturnUrl = returnUrl;
            return View(loginModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            // Define admin and user passwords
            const string adminPassword = "Admin123!";
            const string userPassword = "SalesRep123!";

            // Assign role based on password
            string role = null;
            if (loginModel.Password == adminPassword)
            {
                role = "Admin";
            }
            else if (loginModel.Password == userPassword)
            {
                role = "SalesRep";
            }
            else
            {
                // Handle invalid password
                ViewBag.Message = "Invalid Credentials";
                return View(loginModel);
            }

            // Find user by username
            var user = users.Where(u => u.UserName == loginModel.UserName).FirstOrDefault();

            if (user != null && user.Role == role)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.UserId)),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role), // Corrected claim type
                    new Claim("AzureCustomerOPeration", "Code"),
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                {
                    IsPersistent = loginModel.RememberLogin
                });

                return RedirectToAction("Index", "CustomerDetails");
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
            return RedirectToAction("Index", "Home");
        }
    }
}
