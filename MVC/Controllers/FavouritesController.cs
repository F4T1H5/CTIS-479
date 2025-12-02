using APP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    [Authorize]
    public class FavouritesController : Controller
    {
        private readonly IFavouriteService _favouriteService;

        public FavouritesController(IFavouriteService favouriteService)
        {
            _favouriteService = favouriteService;
        }

        private int GetUserId() => Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == "Id")?.Value);

        public IActionResult Index()
        {
            var favouritesGroupedBy = _favouriteService.GetCartGroupedBy(GetUserId());
            return View(favouritesGroupedBy);
        }

        public IActionResult Clear()
        {
            _favouriteService.ClearCart(GetUserId());
            TempData["Message"] = "Favourites cleared.";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int productId)
        {
            _favouriteService.RemoveFromCart(GetUserId(), productId);
            TempData["Message"] = "Movie removed from favourites.";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Add(int productId)
        {
            _favouriteService.AddToCart(GetUserId(), productId);
            TempData["Message"] = "Movie added to favourites.";
            return RedirectToAction("Index", "Movies");
        }
    }
}
