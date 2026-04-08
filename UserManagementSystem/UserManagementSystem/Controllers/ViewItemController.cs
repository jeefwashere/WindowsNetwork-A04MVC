using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UserManagementSystem.Controllers
{
    public class ViewItemController : Controller
    {
        // GET: ViewItemController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ViewItemController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ViewItemController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ViewItemController/Create
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

        // GET: ViewItemController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ViewItemController/Edit/5
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

        // GET: ViewItemController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ViewItemController/Delete/5
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
