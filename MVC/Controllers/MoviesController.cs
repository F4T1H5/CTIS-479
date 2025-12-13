#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CORE.APP.Services;
using APP.Models;

// Generated from Custom MVC Template.

namespace MVC.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        private readonly IService<MovieRequest, MovieResponse> _movieService;
        private readonly IService<DirectorRequest, DirectorResponse> _directorService;
        private readonly IService<GenreRequest, GenreResponse> _genreService;

        public MoviesController(
            IService<MovieRequest, MovieResponse> movieService,
            IService<DirectorRequest, DirectorResponse> directorService,
            IService<GenreRequest, GenreResponse> genreService
        )
        {
            _movieService = movieService;
            _directorService = directorService;
            _genreService = genreService;
        }

        private void SetViewData()
        {
            ViewData["DirectorId"] = new SelectList(_directorService.List(), "Id", "FullName");

            ViewBag.GenreIds = new MultiSelectList(_genreService.List(), "Id", "Name");
        }

        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
        }

        // GET: Movies
        public IActionResult Index()
        {
            var list = _movieService.List();
            return View(list);
        }

        // GET: Movies/Details/5
        public IActionResult Details(int id)
        {
            var item = _movieService.Item(id);
            return View(item);
        }

        // GET: Movies/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            SetViewData();
            return View();
        }

        // POST: Movies/Create
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(MovieRequest movie)
        {
            if (ModelState.IsValid)
            {
                var response = _movieService.Create(movie);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(movie);
        }

        // GET: Movies/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var item = _movieService.Edit(id);
            SetViewData();
            return View(item);
        }

        // POST: Movies/Edit
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(MovieRequest movie)
        {
            if (ModelState.IsValid)
            {
                var response = _movieService.Update(movie);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(movie);
        }

        // GET: Movies/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var item = _movieService.Item(id);
            return View(item);
        }

        // POST: Movies/Delete
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _movieService.Delete(id);
            SetTempData(response.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}
