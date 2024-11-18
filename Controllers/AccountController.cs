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
        public IActionResult Register() 
        { 
            return View(); 
        } 
        [HttpPost] 
        public async Task<IActionResult> Register(RegisterViewModel model) 
        { if (ModelState.IsValid) 
            { // Check if the user already exists
              var existingUser = users.FirstOrDefault(u => u.UserName == model.Email); 
                if (existingUser != null) 
                { ModelState.AddModelError(string.Empty, "User already exists."); 
                    return View(model); 
                }
                // Create new user
                var newUser = new UserModel
              {   UserId = users.Count + 1, // Simple user ID assignment
                 UserName = model.Email, 
                 Password = model.Password, // Note: Password should be hashed in a real application
                 Role = model.Role ?? "User" // Default role if none specified
              };
                 users.Add(newUser); 
                // Redirect to login or directly log in the user
                var claims = new List<Claim>() 
                {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(newUser.UserId)), 
                    new Claim(ClaimTypes.Name, newUser.UserName), 
                    new Claim(ClaimTypes.Role, newUser.Role), 
                    new Claim("AzureCustomerOPeration", "Code"), 
                }; 
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme); 
                var principal = new ClaimsPrincipal(identity); 

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties() 
                {
                    IsPersistent = true // Change based on your preference
                 }); 
                return RedirectToAction("Index", "CustomerDetails"); 
            }
            return View(model); 
        }
    }
}
