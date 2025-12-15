using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CORE.APP.Services;
using APP.Models;

// Generated from Custom MVC Template.

namespace MVC.Controllers
{
    [Authorize]
    public class GenresController : Controller
    {
        private readonly IService<GenreRequest, GenreResponse> _genreService;

        public GenresController(
			IService<GenreRequest, GenreResponse> genreService

        )
        {
            _genreService = genreService;

        }

        private void SetViewData()
        {
        }

        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
        }

        // GET: Genres
        public IActionResult Index()
        {
            var list = _genreService.List();
            return View(list);
        }
        bool IsOwnAccount(int id)
        {
            return id.ToString() == (User.Claims.SingleOrDefault(claim => claim.Type == "Id")?.Value ?? string.Empty);
        }

        // GET: Genres/Details/5
        public IActionResult Details(int id)
        {
            if (!IsOwnAccount(id) && !User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            var item = _genreService.Item(id);
            return View(item); 
        }

        // GET: Genres/Create
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

        // POST: Genres/Create
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create(GenreRequest genre)
        {
            if (!User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var response = _genreService.Create(genre);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(genre);
        }

        // GET: Genres/Edit/5
        [Authorize]
        public IActionResult Edit(int id)
        {
            if (!IsOwnAccount(id) && !User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            var item = _genreService.Edit(id);
            SetViewData();
            return View(item);
        }

        // POST: Genres/Edit
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(GenreRequest genre)
        {
            if (!User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var response = _genreService.Update(genre);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(genre);
        }

        // GET: Genres/Delete/5
        [Authorize]
        public IActionResult Delete(int id)
        {
            if (!IsOwnAccount(id) && !User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            var item = _genreService.Item(id);
            return View(item);
        }

        // POST: Genres/Delete
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        [Authorize]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsOwnAccount(id) && !User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            var response = _genreService.Delete(id);
            SetTempData(response.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}
