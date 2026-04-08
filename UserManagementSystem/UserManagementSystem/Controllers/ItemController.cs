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
        //Constructor to get database information
        public ItemController(UserContext userContext)
        {
            this.userContext = userContext;
        }

        //GET: AddController


       //POST
       [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(UserItem item)
        {
            if (ModelState.IsValid)
            {
                //item.OwnerID = 1; // replace with session later

                userContext.UserItems.Add(item);
                await userContext.SaveChangesAsync();

                return RedirectToAction("Add", "ViewItem");
            }
            return View(item);
        }
        private static List<string> Items = new List<string>
        {
            "Apple",
            "Banana",
            "Orange"
        };

        public IActionResult Index()
        {
            return View(Items);
        }
        [HttpGet]
        //public IActionResult Add()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Add(string ItemName)
        //{
        //    if (!string.IsNullOrWhiteSpace(ItemName))
        //    {
        //        Items.Add(ItemName);
        //    }

        //    return RedirectToAction("Index");
        //}


        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string ItemName)
        {
            if (!string.IsNullOrWhiteSpace(ItemName))
            {
                Items.Remove(ItemName);
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Update()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(string ItemName)
        {
            if (!string.IsNullOrWhiteSpace(ItemName))
            {
                
            }

            return RedirectToAction("Index");
        }
    }
}
