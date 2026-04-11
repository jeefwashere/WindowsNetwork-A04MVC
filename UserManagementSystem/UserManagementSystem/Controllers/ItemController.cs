using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Data;
using UserManagementSystem.Models;
using System.Security.Claims;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;



namespace UserManagementSystem.Controllers
{
    public class ItemController : Controller
    {
        //Connection Settings
        private readonly UserContext userContext;
        //Constructor to get database information
        public ItemController(UserContext userContext)
        {
            this.userContext = userContext;
        }
        //GET: Show all Items
        [Authorize]
        public async Task<IActionResult> Index()
        {
            IActionResult viewResult = View();

            string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                viewResult = RedirectToAction("Login", "Account"); // Can't use the logout action directly because it's a post action
            }
            else
            {
                List<UserItem> items = await userContext.UserItem
                    .Where(item => item.OwnerID == userId)
                    .Include(item => item.Owner)
                    .ToListAsync();

                viewResult = View(items);
            }

            return viewResult;
        }

        //GET: Add Item page

        [Authorize]
        public IActionResult Add()
        {
            return View();
        }

        //Post Add item logic
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Add(AddViewModel add)
        {
            IActionResult viewResult = View(add);

            if (ModelState.IsValid)
            {
                string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

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
                        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        viewResult = RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        UserItem item = new UserItem();

                        if (add.Quantity < 0)
                        {
                            ModelState.AddModelError("", "Item quantity cannot be less than 0");
                        }
                        else
                        {
                            item.ItemName = add.ItemName;
                            item.Description = string.IsNullOrEmpty(add.Description) ? string.Empty : add.Description;
                            item.Quantity = add.Quantity;
                            item.Owner = currentUser;
                            item.OwnerID = currentUser.UserId; // Test value
                            userContext.UserItem.Add(item);
                            await userContext.SaveChangesAsync();

                            viewResult = RedirectToAction("Index");
                        }

                    }
                }
            }
            return viewResult;
        }



        //Get Item/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            IActionResult viewResult = View();

            UserItem? item = await userContext.UserItem.FindAsync(id);

            if (item == null)
            {
                ModelState.AddModelError("", "Item not Found");
                viewResult = View();
            }

            userContext.UserItem.Remove(item);
            await userContext.SaveChangesAsync();

            viewResult = RedirectToAction("Index");

            return viewResult;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            IActionResult viewResult = View();
            UserItem? item = await userContext.UserItem.FindAsync(id);

            if (item == null)
            {
                viewResult = NotFound();
            }
            else
            {
                UpdateViewModel updateModel = new UpdateViewModel();

                updateModel.ItemID = item.UserItemId;
                updateModel.ItemName = item.ItemName;
                updateModel.Description = item.Description;
                updateModel.Quantity = item.Quantity;

                viewResult = View(updateModel);
            }

            return viewResult;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Update(UpdateViewModel update)
        {
            IActionResult viewResult = View(update);

            if (ModelState.IsValid)
            {
                string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

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
                        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        viewResult = RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        UserItem? userItem = await userContext.UserItem.FindAsync(update.ItemID);

                        if (userItem == null)
                        {
                            viewResult = NotFound();
                        }
                        else
                        {
                            if (userItem.OwnerID != userId)
                            {
                                viewResult = NotFound();
                            }
                            else
                            {
                                if (update.Quantity < 0)
                                {
                                    ModelState.AddModelError("", "Quantity cannot be negative");
                                }
                                else
                                {
                                    userItem.ItemName = update.ItemName;
                                    userItem.Description = string.IsNullOrEmpty(update.Description) ? string.Empty : update.Description;
                                    userItem.Quantity = update.Quantity;

                                    await userContext.SaveChangesAsync();

                                    viewResult = RedirectToAction("Index");
                                }
                            }
                        }
                    }
                }
            }
            return viewResult;
        }

    }
}
