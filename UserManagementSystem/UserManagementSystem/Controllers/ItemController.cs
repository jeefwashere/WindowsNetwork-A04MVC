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

/*
   * FILE : ItemController.cs
   * PROGRAMMER : Name(s): Josiah Williams,Jeff, Gao Ricardo
   * DESCRIPTION :this is item controller to controller show item page
   * 
   */

namespace UserManagementSystem.Controllers
{
    public class ItemController : Controller
    {
        //Connection Settings
        private readonly UserContext userContext;
        //Constructor to get database information
        /// <summary>
        /// set database connection for item controller
        /// </summary>
        /// <param name="userContext"></param>
        public ItemController(UserContext userContext)
        {
            this.userContext = userContext;
        }
        /// <summary>
        /// first index show all but need authorize
        /// </summary>
        /// <returns></returns>
        //GET: Show all Items
        [Authorize]
        public async Task<IActionResult> Index()
        {
            IActionResult viewResult = View();
            //match the info
            string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //get id
            if (!int.TryParse(userIdClaim, out int userId))
            {//no corrrst login out 
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                viewResult = RedirectToAction("Login", "Account"); // Can't use the logout action directly because it's a post action
            }
            else
            {//sql the itemOwnerID==userId
                //include means at same time gei it owner
                //transfer to a list 
                List<UserItem> items = await userContext.UserItem
                    .Where(item => item.OwnerID == userId)
                    .Include(item => item.Owner)
                    .ToListAsync();

                viewResult = View(items);
            }

            return viewResult;
        }

        //GET: Add Item page
        /// <summary>
        /// add page
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Add()
        {
            return View();
        }
        /// <summary>
        ///         Post Add item logic
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Add(AddViewModel add)
        {//return to call Add and pass add
            IActionResult viewResult = View(add);

            if (ModelState.IsValid)
            {// if all good
                string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!int.TryParse(userIdClaim, out int userId))
                {//if bad signout
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    viewResult = RedirectToAction("Login", "Account");
                }
                else
                {
                    //found id 
                    User? currentUser = await userContext.User.FindAsync(userId);

                    if (currentUser == null)
                    {// if not found user
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
                        {// if all good get into the item and add to database
                            item.ItemName = add.ItemName;
                            item.Description = string.IsNullOrEmpty(add.Description) ? string.Empty : add.Description;
                            item.Quantity = add.Quantity;
                            item.Owner = currentUser;
                            item.OwnerID = currentUser.UserId; // Test value
                            userContext.UserItem.Add(item);
                            await userContext.SaveChangesAsync();
                            //return to local index
                            viewResult = RedirectToAction("Index");
                        }

                    }
                }
            }
            return viewResult;
        }


        /// <summary>
        /// delete action
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //Get Item/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            //not pass any
            IActionResult viewResult = View();

            UserItem? item = await userContext.UserItem.FindAsync(id);

            if (item == null)
            {
                ModelState.AddModelError("", "Item not Found");
                viewResult = View();
            }
            //remove
            userContext.UserItem.Remove(item);
            await userContext.SaveChangesAsync();
            //back
            viewResult = RedirectToAction("Index");

            return viewResult;
        }
        /// <summary>
        /// updatge the data base
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            //back the update page view
            IActionResult viewResult = View();
            UserItem? item = await userContext.UserItem.FindAsync(id);

            if (item == null)
            {
                viewResult = NotFound();
            }
            else
            {// new instance
                UpdateViewModel updateModel = new UpdateViewModel();

                updateModel.ItemID = item.UserItemId;
                updateModel.ItemName = item.ItemName;
                updateModel.Description = item.Description;
                updateModel.Quantity = item.Quantity;
                //update info 
                viewResult = View(updateModel);
                //go to it
            }

            return viewResult;
        }
        /// <summary>
        /// real update for post
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Update(UpdateViewModel update)
        {
            IActionResult viewResult = View(update);

            if (ModelState.IsValid)
            {//check if valid
                string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!int.TryParse(userIdClaim, out int userId))
                {//check people
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    viewResult = RedirectToAction("Login", "Account");
                }
                else
                {//check found user
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
                                {// update logic
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
