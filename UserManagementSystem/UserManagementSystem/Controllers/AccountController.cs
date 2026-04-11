using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using UserManagementSystem.Data;
using UserManagementSystem.Models;

namespace UserManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        // DB Connection
        private readonly UserContext userContext;

        public AccountController(UserContext userContext)
        {
            this.userContext = userContext;
        }

        // GET: AccountController
        [AllowAnonymous]
        public ActionResult Index(string? returnUrl = null)
        {
            LoginViewModel login = new LoginViewModel();

            login.ReturnUrl = returnUrl;

            // Store the URL the user was trying to access (to redirect after login)  
            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        // POST: LoginController/Login/5
        public async Task<IActionResult> Login(LoginViewModel login, string submitButton)
        {
            // Forms Authentication found here: https://www.dotnettutorial.co.in/2025/07/authentication-in-aspnet-mvc-with-example.html
            IActionResult viewResult = View("Index", login);


            // Login structure based on: https://www.w3tutorials.net/blog/asp-net-core-simplest-possible-forms-authentication/#project-setup
            login.ReturnUrl ??= Url.Content("~/");

            if (submitButton == "Register")
            {
                viewResult = RedirectToAction("Register", "Account");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    User? user = await userContext.User.FirstOrDefaultAsync(user => user.Username == login.Username);

                    if (user != null)
                    {
                        // Password Hasher found here: https://medium.com/@nambi2210/password-hashing-in-asp-net-core-ee377c29fa24
                        // Explored further in documentation: https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.passwordhasher-1?view=aspnetcore-10.0
                        PasswordHasher<User> hasher = new PasswordHasher<User>();
                        PasswordVerificationResult passwordCheck = hasher.VerifyHashedPassword(user, user.HashedPassword, login.Password);

                        if (passwordCheck != PasswordVerificationResult.Failed)
                        {
                            List<Claim> claims = new List<Claim>();
                            claims.Add(new Claim(ClaimTypes.Name, user.Username.ToString()));
                            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()));

                            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                                claims,
                                CookieAuthenticationDefaults.AuthenticationScheme);

                            AuthenticationProperties authProperties = new AuthenticationProperties();
                            authProperties.IsPersistent = false;
                            authProperties.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30);

                            await HttpContext.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(claimsIdentity),
                                authProperties);

                            if (!string.IsNullOrEmpty(login.ReturnUrl))
                            {
                                viewResult = LocalRedirect(login.ReturnUrl);
                            }
                            else
                            {
                                viewResult = RedirectToAction("Index", "Item");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid Password");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid User");
                    }
                }
            }

            return viewResult;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            IActionResult viewResult = View(register);

            if (ModelState.IsValid)
            {
                bool userCheck = await userContext.User.AnyAsync(u => u.Username == register.Username);

                if (userCheck)
                {
                    ModelState.AddModelError("", "Username already exists");
                }
                else if (!Regex.IsMatch(register.PostalCode, "\\A[ABCEGHJKLMNPRSTVXY]\\d[A-Z] ?\\d[A-Z]\\d\\z"))
                {
                    ModelState.AddModelError("", "Invalid postal code");
                }
                else
                {
                    User user = new User();
                    user.Username = register.Username;
                    user.StreetAddress = register.StreetName;
                    user.City = register.City;
                    user.Province = register.Province;
                    user.Country = register.Country;
                    user.PostalCode = register.PostalCode;

                    PasswordHasher<User> hasher = new PasswordHasher<User>();
                    string hash = hasher.HashPassword(user, register.Password);

                    if (!string.IsNullOrEmpty(hash))
                    {
                        user.HashedPassword = hash;

                        userContext.User.Add(user);
                        await userContext.SaveChangesAsync();

                        viewResult = RedirectToAction("Index", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid password");
                    }
                }
            }

            return viewResult;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
