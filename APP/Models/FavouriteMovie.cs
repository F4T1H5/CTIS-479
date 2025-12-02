using System.ComponentModel;

namespace APP.Models
{
    public class FavouriteMovie
    {
        public int UserId { get; set; }

        public int MovieId { get; set; }

        [DisplayName("Movie Name")]
        public string MovieName { get; set; }

        public DateTime? ReleaseDate { get; set; }

        [DisplayName("Release Date")]
        public string ReleaseDateF { get; set; }
    }
}
