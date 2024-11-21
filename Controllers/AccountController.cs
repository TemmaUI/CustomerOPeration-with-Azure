using AzureCustomerOPeration.Models;
using AzureCustomerOPeration.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AzureCustomerOPeration.Controllers
{
    public class AccountController : Controller
    {
        private readonly EmailService _emailService;
        public List<UserModel> users = null;

        public AccountController(EmailService emailService)
        {
            _emailService = emailService;
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
                if (!user.EmailConfirmed)
                {
                    ViewBag.Message = "Please confirm your email before logging in.";
                    return View(loginModel);
                }

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.UserId)),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role),
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
        {
            System.Diagnostics.Debug.WriteLine("Register action called");

            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("Model is valid");

                // Check if user already exists
                var existingUser = users.FirstOrDefault(u => u.UserName == model.Email);
                if (existingUser != null)
                {
                    System.Diagnostics.Debug.WriteLine("User already exists");
                    ModelState.AddModelError(string.Empty, "User already exists.");
                    return View(model);
                }

                // Create new user
                var newUser = new UserModel
                {
                    UserId = users.Count + 1,
                    UserName = model.Email,
                    Password = model.Password,
                    Role = model.Role ?? "User",
                    EmailConfirmed = false
                };
                users.Add(newUser);

                // Generate email confirmation token
                var token = Guid.NewGuid().ToString();

                // Send confirmation email
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = model.Email }, Request.Scheme);
                await _emailService.SendEmailAsync(model.Email, "Confirm your email", $"Please confirm your email by clicking this link: {confirmationLink}");

                // Store the token for later verification
                TempData["EmailConfirmationToken"] = token;

                return RedirectToAction("Details", "CustomerDetails");
            }

            System.Diagnostics.Debug.WriteLine("Model is invalid");
            return View(model);
        }


        [HttpGet]
        public IActionResult ConfirmEmail(string token, string email)
        {
            var storedToken = TempData["EmailConfirmationToken"] as string;

            if (storedToken == token)
            {
                var user = users.FirstOrDefault(u => u.UserName == email);
                if (user != null)
                {
                    // Update the user's record to mark the email as confirmed
                    user.EmailConfirmed = true;
                    return View("ConfirmEmail");
                }
            }

            return View("Error");
        }
    }
}