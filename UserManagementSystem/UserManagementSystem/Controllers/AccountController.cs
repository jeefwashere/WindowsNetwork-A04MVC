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

        // GET: LoginController
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
        // GET: LoginController/Login/5
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            // Forms Authentication found here: https://www.dotnettutorial.co.in/2025/07/authentication-in-aspnet-mvc-with-example.html
            IActionResult viewResult = View("Index", login);

            login.ReturnUrl ??= Url.Content("~/");

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
                        claims.Add(new Claim(ClaimTypes.Name, user.Username));

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
                        ModelState.AddModelError("Password", "Invalid Password");
                    }
                }
                else
                {
                    ModelState.AddModelError("Username", "Invalid User");
                }
            }

            return viewResult;
        }

        [HttpPost]
        public async Task<IActionResult> Register()
        {
            if (ModelState.IsValid)
            {

            }
        }
    }
}
