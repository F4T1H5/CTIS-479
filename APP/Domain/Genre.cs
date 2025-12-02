using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Domain
{
    public class Genre : Entity
    {
        [Required, StringLength(50)]
        public string Name { get; set; }

        public List<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();

        [NotMapped]
        public List<int> MovieIds
        {
            get => MovieGenres.Select(movieGenreEntity => movieGenreEntity.MovieId).ToList();
            set => MovieGenres = value?.Select(movieId => new MovieGenre() { MovieId = movieId }).ToList();
        }
    }
}