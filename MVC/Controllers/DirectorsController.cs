using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CORE.APP.Services;
using APP.Models;

// Generated from Custom MVC Template.

namespace MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DirectorsController : Controller
    {
        private readonly IService<DirectorRequest, DirectorResponse> _directorService;

        public DirectorsController(
			IService<DirectorRequest, DirectorResponse> directorService
        )
        {
            _directorService = directorService;
        }

        private void SetViewData()
        {
        }

        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
        }

        // GET: Directors
        public IActionResult Index()
        {
            var list = _directorService.List();
            return View(list);
        }

        // GET: Directors/Details/5
        public IActionResult Details(int id)
        {
            var item = _directorService.Item(id);
            return View(item);
        }

        // GET: Directors/Create
        public IActionResult Create()
        {
            SetViewData();
            return View();
        }

        // POST: Directors/Create
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(DirectorRequest director)
        {
            if (ModelState.IsValid)
            {
                var response = _directorService.Create(director);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message); 
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(director);
        }

        // GET: Directors/Edit/5
        public IActionResult Edit(int id)
        {
            var item = _directorService.Edit(id);
            SetViewData();
            return View(item);
        }

        // POST: Directors/Edit
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(DirectorRequest director)
        {
            if (ModelState.IsValid)
            {
                var response = _directorService.Update(director);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(director);
        }

        // GET: Directors/Delete/5
        public IActionResult Delete(int id)
        {
            var item = _directorService.Item(id);
            return View(item);
        }

        // POST: Directors/Delete
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _directorService.Delete(id);
            SetTempData(response.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}
