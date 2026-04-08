using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Data;
using UserManagementSystem.Models;



namespace UserManagementSystem.Controllers
{
    public class ItemController : Controller
    {
        //Connection Settings
        private readonly UserContext userContext;
        // Constructor to get database information
        public ItemController(UserContext userContext)
        {
            this.userContext = userContext;
        }

        // GET: AddItemController
        public ActionResult Index()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UserItem item)
        {
            if(ModelState.IsValid)
            {
                //item.OwnerID = 1; // replace with session later

                userContext.UserItems.Add(item);
                await userContext.SaveChangesAsync();

                return RedirectToAction("Index", "Item");
            }
            return View(item);
        }

        
    }
}
