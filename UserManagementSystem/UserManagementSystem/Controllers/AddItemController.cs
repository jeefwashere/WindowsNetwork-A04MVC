using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UserManagementSystem.Controllers
{
    public class AddItemController : Controller
    {
        // GET: AddItemController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AddItemController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AddItemController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AddItemController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AddItemController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AddItemController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AddItemController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AddItemController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
