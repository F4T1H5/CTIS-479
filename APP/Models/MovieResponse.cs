using CORE.APP.Models;
using System.ComponentModel;

namespace APP.Models
{
    public class MovieResponse : Response
    {
        [DisplayName("Name")]
        public string Name { get; set; }

        public DateTime? ReleaseDate { get; set; }

        [DisplayName("Total Revenue")]
        public decimal TotalRevenue { get; set; }

        public int DirectorId { get; set; }

        [DisplayName("Release Date")]
        public string ReleaseDateF { get; set; }

        [DisplayName("Total Revenue")]
        public string TotalRevenueF { get; set; }

        [DisplayName("Director")]
        public string Director { get; set; }

        public List<string> Genres { get; set; }

        public List<int> GenreIds { get; set; }

        [DisplayName("Genres")]
        public List<GenreResponse> GenreList { get; set; }
    }
}
