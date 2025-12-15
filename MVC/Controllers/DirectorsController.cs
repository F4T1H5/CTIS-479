using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CORE.APP.Services;
using APP.Models;

// Generated from Custom MVC Template.

namespace MVC.Controllers
{
    [Authorize]
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

        bool IsOwnAccount(int id)
        {
            return id.ToString() == (User.Claims.SingleOrDefault(claim => claim.Type == "Id")?.Value ?? string.Empty);
        }

        // GET: Directors/Details/5
        public IActionResult Details(int id)
        {
            if (!IsOwnAccount(id) && !User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            var item = _directorService.Item(id);
            return View(item);
        }

        // GET: Directors/Create
        [Authorize]
        public IActionResult Create()
        {
            if (!User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            SetViewData();
            return View();
        }

        // POST: Directors/Create
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create(DirectorRequest director)
        {
            if (!User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

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
        [Authorize]
        public IActionResult Edit(int id)
        {
            if (!IsOwnAccount(id) && !User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            var item = _directorService.Edit(id);
            SetViewData();
            return View(item);
        }

        // POST: Directors/Edit
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(DirectorRequest director)
        {
            if (!User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

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
        [Authorize]
        public IActionResult Delete(int id)
        {
            if (!IsOwnAccount(id) && !User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            var item = _directorService.Item(id);
            return View(item);
        }

        // POST: Directors/Delete
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        [Authorize]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsOwnAccount(id) && !User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            var response = _directorService.Delete(id);
            SetTempData(response.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}
