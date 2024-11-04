using AzureCustomerOPeration.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace AzureCustomerOPeration.Controllers
{
    public class AdminRole : Controller
    {
        [HttpGet]
        public IActionResult AdminPriviledge()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminPriviledge(LoginModel model)
        {
            var result = LoginUser(model.Username, model.Password);

            switch (result)
            {
                case LoginResult.Success:
                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, model.Username) };
                    var userIdentity = new ClaimsIdentity(claims, "login");
                    var userPrincipal = new ClaimsPrincipal(userIdentity);

                    var loginProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                    };

                    await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, loginProperties);

                    return RedirectToAction("Index", "CustomerDetails");
                case LoginResult.InvalidInput:
                    ModelState.AddModelError(string.Empty, "Invalid input.");
                    break;
                case LoginResult.WrongUsername:
                    ModelState.AddModelError(string.Empty, "Incorrect entry, please try again.");
                    break;
                case LoginResult.WrongPassword:
                    ModelState.AddModelError(string.Empty, "Incorrect entry, please try again.");
                    break;
            }

            return View(model);
        }

        private LoginResult LoginUser(object username, object password)
        {
            string AddpredefinedUsername = "admin";
            string AddpredefinedPassword = "123456";

            if (username == null || password == null)
            {
                return LoginResult.InvalidInput;
            }

            string userName = (string?)username;
            string passWord = (string?)password;

            if (!string.Equals(userName, AddpredefinedUsername, StringComparison.OrdinalIgnoreCase))
            {
                return LoginResult.WrongUsername;
            }

            if (!string.Equals(passWord, AddpredefinedPassword, StringComparison.OrdinalIgnoreCase))
            {
                return LoginResult.WrongPassword;
            }

            return LoginResult.Success;
        }

        public enum LoginResult
        {
            Success,
            InvalidInput,
            WrongUsername,
            WrongPassword
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