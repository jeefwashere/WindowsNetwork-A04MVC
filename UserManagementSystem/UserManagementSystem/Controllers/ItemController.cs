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
            var items = await userContext.UserItem.ToListAsync();
            return View(items);
        }
        
        //GET: Add Item page

        public IActionResult Add()
        {
            return View();
        }

        //Post Add item logic
        [HttpPost] 
        public async Task<IActionResult> Add(UserItem item)
        {
            if (ModelState.IsValid)
            {
                item.OwnerID = 1; // Test value
                userContext.UserItem.Add(item);
                await userContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(item);
        }

    }
}
