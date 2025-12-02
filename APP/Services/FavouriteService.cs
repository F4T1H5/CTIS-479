using APP.Models;
using CORE.APP.Services;
using CORE.APP.Services.Session.MVC;

namespace APP.Services
{
    public class FavouriteService : IFavouriteService
    {
        const string SESSIONKEY = "favourites";

        private readonly SessionServiceBase _sessionService;

        private readonly IService<MovieRequest, MovieResponse> _movieService;

        public FavouriteService(SessionServiceBase sessionService, IService<MovieRequest, MovieResponse> movieService)
        {
            _movieService = movieService;
            _sessionService = sessionService;
        }

        public List<FavouriteMovie> GetCart(int userId)
        {
            var favourites = _sessionService.GetSession<List<FavouriteMovie>>(SESSIONKEY);
            if (favourites is not null)
                return favourites.Where(f => f.UserId == userId).ToList();
            return new List<FavouriteMovie>();
        }

        public List<FavouriteMovieGroupedBy> GetCartGroupedBy(int userId)
        {
            var favourites = GetCart(userId);

            return favourites
                .GroupBy(favouriteMovie => new
                {
                    favouriteMovie.UserId,
                    favouriteMovie.MovieId,
                    favouriteMovie.MovieName
                })
                .Select(favouriteMovieGroupedBy => new FavouriteMovieGroupedBy
                {
                    UserId = favouriteMovieGroupedBy.Key.UserId,
                    MovieId = favouriteMovieGroupedBy.Key.MovieId,
                    MovieName = favouriteMovieGroupedBy.Key.MovieName,
                    MovieCount = favouriteMovieGroupedBy.Count()
                }).ToList();
        }

        public void AddToCart(int userId, int productId)
        {
            var movie = _movieService.Item(productId);
            if (movie is not null)
            {
                var favourites = GetCart(userId);
                favourites.Add(new FavouriteMovie
                {
                    UserId = userId,
                    MovieId = movie.Id,
                    MovieName = movie.Name,
                    ReleaseDate = movie.ReleaseDate,
                    ReleaseDateF = movie.ReleaseDateF
                });
                _sessionService.SetSession(SESSIONKEY, favourites);
            }
        }

        public void RemoveFromCart(int userId, int productId)
        {
            var favourites = GetCart(userId);
            var favouriteMovie = favourites.FirstOrDefault(f => f.UserId == userId && f.MovieId == productId);
            if (favouriteMovie is not null)
                favourites.Remove(favouriteMovie);
            _sessionService.SetSession(SESSIONKEY, favourites);
        }

        public void ClearCart(int userId)
        {
            var favourites = GetCart(userId);
            favourites.RemoveAll(f => f.UserId == userId);
            _sessionService.SetSession(SESSIONKEY, favourites);
        }
    }
}
