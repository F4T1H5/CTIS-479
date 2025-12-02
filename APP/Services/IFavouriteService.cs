using APP.Models;

namespace APP.Services
{
    public interface IFavouriteService
    {
        public List<FavouriteMovie> GetCart(int userId);

        public List<FavouriteMovieGroupedBy> GetCartGroupedBy(int userId);

        public void AddToCart(int userId, int productId);

        public void RemoveFromCart(int userId, int productId);

        public void ClearCart(int userId);
    }
}
