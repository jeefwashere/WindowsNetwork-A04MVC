using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Data;
using UserManagementSystem.Models;



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

        public async Task<IActionResult> Index()
        {
            List<UserItem> items = await userContext.UserItem.ToListAsync();
            return View(items);
        }

        //GET: Add Item page

        public IActionResult Add()
        {
            return View();
        }

        //Post Add item logic
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(UserItem item)
        {
            if (ModelState.IsValid)
            {
                User user = new User();

                item.Owner = user;
                item.OwnerID = user.UserId; // Test value
                userContext.UserItem.Add(item);
                await userContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(item);
        }



        //Get Item/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
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
