using System.ComponentModel;

namespace APP.Models
{
    public class FavouriteMovieGroupedBy
    {
        public int UserId { get; set; }

        public int MovieId { get; set; }

        [DisplayName("Movie Name")]
        public string MovieName { get; set; }

        [DisplayName("Movie Count")]
        public int MovieCount { get; set; }
    }
}
