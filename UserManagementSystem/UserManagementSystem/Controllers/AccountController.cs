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
        //this means no authentication can access
        [AllowAnonymous]
        public ActionResult Index(string? returnUrl = null)
        {   //create a new view obejct for login
            LoginViewModel login = new LoginViewModel();
            //update info
            login.ReturnUrl = returnUrl;

            // Store the URL the user was trying to access (to redirect after login)  
            //pass to view
            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]//detect bad request
        // POST: LoginController/Login/5
        public async Task<IActionResult> Login(LoginViewModel login, string submitButton)
        {
            // Forms Authentication found here: https://www.dotnettutorial.co.in/2025/07/authentication-in-aspnet-mvc-with-example.html
            IActionResult viewResult = View("Index", login);
            //go to index page and passing back login

            // Login structure based on: https://www.w3tutorials.net/blog/asp-net-core-simplest-possible-forms-authentication/#project-setup
            login.ReturnUrl ??= Url.Content("~/");
            // if returnurl is null means set up to the root place 
            //this get from fronter
            if (submitButton == "Register")
            {   // if is the register page means return to register not validate
                viewResult = RedirectToAction("Register", "Account");
            }
            else
            {
                if (ModelState.IsValid)//this check is form valid
                {
                    //found the first match info 
                    User? user = await userContext.User.FirstOrDefaultAsync(user => user.Username == login.Username);

                    if (user != null)
                    {
                        // Password Hasher found here: https://medium.com/@nambi2210/password-hashing-in-asp-net-core-ee377c29fa24
                        // Explored further in documentation: https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.passwordhasher-1?view=aspnetcore-10.0
                        PasswordHasher<User> hasher = new PasswordHasher<User>();
                        PasswordVerificationResult passwordCheck = hasher.VerifyHashedPassword(user, user.HashedPassword, login.Password);
                        //check hash match
                        if (passwordCheck != PasswordVerificationResult.Failed)
                        {
                            List<Claim> claims = new List<Claim>();
                            claims.Add(new Claim(ClaimTypes.Name, user.Username.ToString()));
                            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()));
                            //for this people set up a validateion info
                            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                                claims,
                                CookieAuthenticationDefaults.AuthenticationScheme);
                            //set login cookie
                            AuthenticationProperties authProperties = new AuthenticationProperties();
                            authProperties.IsPersistent = false;
                            authProperties.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30);
                            //expire 30 mintutes
                            //write the cookie to those info
                            await HttpContext.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(claimsIdentity),
                                authProperties);

                            if (!string.IsNullOrEmpty(login.ReturnUrl))
                            {
                                viewResult = LocalRedirect(login.ReturnUrl);
                            }
                            else//go back to reguar
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
        //show the register page
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }
        // if post have someone want register
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
                {//already have user
                    ModelState.AddModelError("", "Username already exists");
                }//wrong input for postalcode
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
                    //hash the password
                    PasswordHasher<User> hasher = new PasswordHasher<User>();
                    string hash = hasher.HashPassword(user, register.Password);

                    if (!string.IsNullOrEmpty(hash))
                    {
                        user.HashedPassword = hash;

                        userContext.User.Add(user);
                        await userContext.SaveChangesAsync();// write into database

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
        /// <summary>
        /// this will readirect to home page
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {//release the cookie and go back to home page
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
