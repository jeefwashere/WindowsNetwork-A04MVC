using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using UserManagementSystem.Models;
using UserManagementSystem.Data;

namespace UserManagementSystem.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserContext userContext;

        public ProfileController(UserContext userContext)
        {
            this.userContext = userContext;
        }

        [HttpGet]
        [Authorize]
        // GET: ProfileController
        public async Task<IActionResult> Index()
        {
            IActionResult viewResult = View();

            string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                viewResult = RedirectToAction("Login", "Account"); // Can't use the logout action directly because it's a post action
            }
            else
            {
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    viewResult = RedirectToAction("Login", "Account");
                }
                else
                {
                    User? currentUser = await userContext.User.FindAsync(userId);

                    if (currentUser == null)
                    {
                        viewResult = RedirectToAction("Logout", "Account");
                    }
                    else
                    {
                        ProfileViewModel userProfile = new ProfileViewModel();

                        userProfile.Username = currentUser.Username;
                        userProfile.StreetAddress = currentUser.StreetAddress;
                        userProfile.City = currentUser.City;
                        userProfile.Province = currentUser.Province;
                        userProfile.Country = currentUser.Country;
                        userProfile.PostalCode = currentUser.PostalCode;

                        viewResult = View(userProfile);
                    }
                }
            }
            

            return viewResult;
        }
    }
}
