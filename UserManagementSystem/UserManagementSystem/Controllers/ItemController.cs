using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Data;
using UserManagementSystem.Models;
using System.Security.Claims;
using System.Runtime.CompilerServices;



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
                viewResult = Unauthorized();
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
        public async Task<IActionResult> Add(UserItem item)
        {
            IActionResult viewResult = View(item);

            if (ModelState.IsValid)
            {
                string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    viewResult = Unauthorized();
                }
                else
                {

                    User? currentUser = await userContext.User.FindAsync(userId);

                    if (currentUser == null)
                    {
                        viewResult = Unauthorized();
                    }
                    else
                    {
                        item.Owner = currentUser;
                        item.OwnerID = currentUser.UserId; // Test value
                        userContext.UserItem.Add(item);
                        await userContext.SaveChangesAsync();

                        viewResult = RedirectToAction("Index");
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

            UserItem? item = await userContext.UserItem.FindAsync(id);

            if (item == null)
            {
                ModelState.AddModelError("", "Item not Found");
                return View();
            }

            userContext.UserItem.Remove(item);
            await userContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            UserItem? item = await userContext.UserItem.FindAsync(id);

            if(item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Update(UserItem item)
        {
            if(ModelState.IsValid)
            {
                userContext.UserItem.Update(item);
                await userContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(item);
        }

    }
}
