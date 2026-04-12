using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using UserManagementSystem.Models;
using UserManagementSystem.Data;
/*
   * FILE : ProfileController.cs
   * PROGRAMMER : Name(s): Josiah Williams,Jeff, Gao Ricardo
   * DESCRIPTION :this show profile page for user and show the user info in the profile page
   * 
   */
namespace UserManagementSystem.Controllers
{
    public class ProfileController : Controller
    {//connect database
        private readonly UserContext userContext;
        /// <summary>
        /// set data
        /// </summary>
        /// <param name="userContext"></param>
        public ProfileController(UserContext userContext)
        {
            this.userContext = userContext;
        }
        /// <summary>
        /// get for show info about the profile
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        // GET: ProfileController
        public async Task<IActionResult> Index()
        {
            IActionResult viewResult = View();
            // get id value
            string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                viewResult = RedirectToAction("Login", "Account"); // Can't use the logout action directly because it's a post action
            }
            else
            {//get those info
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
                        //show info
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
