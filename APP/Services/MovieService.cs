using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    public class MovieService : Service<Movie>, IService<MovieRequest, MovieResponse>
    {
        public MovieService(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Movie> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking)
                .Include(m => m.Director)
                .Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre)
                .OrderBy(m => m.Name);
        }

        public CommandResponse Create(MovieRequest request)
        {
            var name = request?.Name?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                return Error("Name is required!");

            if (Query().Any(m => m.Name == name))
                return Error("Movie with the same name exists!");

            if (!Query<Director>().Any(d => d.Id == request.DirectorId))
                return Error("Director not found!");

            var genreIds = (request.GenreIds ?? []).Distinct().ToList();
            if (!genreIds.Any())
                return Error("At least one genre must be selected!");

            var existingGenreCount = Query<Genre>().Count(g => genreIds.Contains(g.Id));
            if (existingGenreCount != genreIds.Count)
                return Error("One or more selected genres were not found!");

            var entity = new Movie
            {
                Name = name,
                ReleaseDate = request.ReleaseDate,
                TotalRevenue = request.TotalRevenue,
                DirectorId = request.DirectorId,

                MovieGenres = genreIds.Select(genreId => new MovieGenre
                {
                    GenreId = genreId
                }).ToList()
            };

            Create(entity);
            return Success("Movie created successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false).SingleOrDefault(m => m.Id == id);
            if (entity is null)
                return Error("Movie not found!");

            Delete(entity.MovieGenres);
            Delete(entity);

            return Success("Movie deleted successfully.", entity.Id);
        }

        public MovieRequest Edit(int id)
        {
            var entity = Query().SingleOrDefault(m => m.Id == id);
            if (entity is null)
                return null;

            return new MovieRequest
            {
                Id = entity.Id,
                Name = entity.Name,
                ReleaseDate = entity.ReleaseDate,
                TotalRevenue = entity.TotalRevenue,
                DirectorId = entity.DirectorId,
                GenreIds = entity.MovieGenres.Select(mg => mg.GenreId).ToList()
            };
        }

        public MovieResponse Item(int id)
        {
            var entity = Query().SingleOrDefault(m => m.Id == id);
            if (entity is null)
                return null;

            return new MovieResponse
            {
                Id = entity.Id,
                Guid = entity.Guid,
                Name = entity.Name,
                ReleaseDate = entity.ReleaseDate,
                TotalRevenue = entity.TotalRevenue,
                DirectorId = entity.DirectorId,
                GenreIds = entity.MovieGenres.Select(mg => mg.GenreId).ToList(),

                ReleaseDateF = entity.ReleaseDate.HasValue ? entity.ReleaseDate.Value.ToString("MM/dd/yyyy") : string.Empty,
                TotalRevenueF = entity.TotalRevenue.ToString("N2"),
                Director = entity.Director != null ? (entity.Director.FirstName + " " + entity.Director.LastName) : string.Empty,
                Genres = entity.MovieGenres.Select(mg => mg.Genre.Name).ToList()
            };
        }

        public List<MovieResponse> List()
        {
            return Query().Select(m => new MovieResponse
            {
                Id = m.Id,
                Guid = m.Guid,
                Name = m.Name,
                ReleaseDate = m.ReleaseDate,
                TotalRevenue = m.TotalRevenue,
                DirectorId = m.DirectorId,
                GenreIds = m.MovieGenres.Select(mg => mg.GenreId).ToList(),

                ReleaseDateF = m.ReleaseDate.HasValue ? m.ReleaseDate.Value.ToString("MM/dd/yyyy") : string.Empty,
                TotalRevenueF = m.TotalRevenue.ToString("N2"),
                Director = m.Director != null ? (m.Director.FirstName + " " + m.Director.LastName) : string.Empty,
                Genres = m.MovieGenres.Select(mg => mg.Genre.Name).ToList()
            }).ToList();
        }

        public CommandResponse Update(MovieRequest request)
        {
            var name = request?.Name?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                return Error("Name is required!");

            if (Query().Any(m => m.Id != request.Id && m.Name == name))
                return Error("Movie with the same name exists!");

            if (!Query<Director>().Any(d => d.Id == request.DirectorId))
                return Error("Director not found!");

            var genreIds = (request.GenreIds ?? []).Distinct().ToList();
            if (!genreIds.Any())
                return Error("At least one genre must be selected!");

            var existingGenreCount = Query<Genre>().Count(g => genreIds.Contains(g.Id));
            if (existingGenreCount != genreIds.Count)
                return Error("One or more selected genres were not found!");

            var entity = Query(false).SingleOrDefault(m => m.Id == request.Id);
            if (entity is null)
                return Error("Movie not found!");

            Delete(entity.MovieGenres);

            entity.Name = name;
            entity.ReleaseDate = request.ReleaseDate;
            entity.TotalRevenue = request.TotalRevenue;
            entity.DirectorId = request.DirectorId;

            entity.MovieGenres = genreIds.Select(genreId => new MovieGenre
            {
                MovieId = entity.Id,
                GenreId = genreId
            }).ToList();

            Update(entity);
            return Success("Movie updated successfully.", entity.Id);
        }
    }
}
