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

        // GET: Genres/Details/5
        public IActionResult Details(int id)
        {
            var item = _genreService.Item(id);
            return View(item); 
        }

        // GET: Genres/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            SetViewData();
            return View();
        }

        // POST: Genres/Create
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(GenreRequest genre)
        {
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
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var item = _genreService.Edit(id);
            SetViewData();
            return View(item);
        }

        // POST: Genres/Edit
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(GenreRequest genre)
        {
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
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var item = _genreService.Item(id);
            return View(item);
        }

        // POST: Genres/Delete
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _genreService.Delete(id);
            SetTempData(response.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}
