using CORE.APP.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class MovieRequest : Request
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        public string Name { get; set; }

        [DisplayName("Release Date")]
        public DateTime? ReleaseDate { get; set; }

        [Range(0, 10000000000, ErrorMessage = "{0} must be between {1} and {2}!")]
        public decimal TotalRevenue{ get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("Director")]
        public int DirectorId { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("Genres")]
        public List<int> GenreIds { get; set; }
    }
}
