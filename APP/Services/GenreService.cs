using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    public class GenreService : Service<Genre>, IService<GenreRequest, GenreResponse>
    {
        public GenreService(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Genre> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking)
                .Include(g => g.MovieGenres).ThenInclude(mg => mg.Movie)
                .OrderBy(g => g.Name);
        }

        public CommandResponse Create(GenreRequest request)
        {
            var name = request?.Name?.Trim();

            if (string.IsNullOrWhiteSpace(name))
                return Error("Name is required!");

            if (Query().Any(g => g.Name == name))
                return Error("Genre with the same name exists!");

            var entity = new Genre
            {
                Name = name
            };

            Create(entity);
            return Success("Genre created successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false).SingleOrDefault(g => g.Id == id);
            if (entity is null)
                return Error("Genre not found!");

            Delete(entity.MovieGenres);
            Delete(entity);

            return Success("Genre deleted successfully.", entity.Id);
        }

        public GenreRequest Edit(int id)
        {
            var entity = Query().SingleOrDefault(g => g.Id == id);
            if (entity is null)
                return null;

            return new GenreRequest
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public GenreResponse Item(int id)
        {
            var entity = Query().SingleOrDefault(g => g.Id == id);
            if (entity is null)
                return null;

            return new GenreResponse
            {
                Id = entity.Id,
                Guid = entity.Guid,
                Name = entity.Name,
                MovieCount = entity.MovieGenres.Count,
                Movies = string.Join("<br>", entity.MovieGenres.Select(mg => mg.Movie.Name))
            };
        }

        public List<GenreResponse> List()
        {
            return Query().Select(g => new GenreResponse
            {
                Id = g.Id,
                Guid = g.Guid,
                Name = g.Name,
                MovieCount = g.MovieGenres.Count,
                Movies = string.Join(", ", g.MovieGenres.Select(mg => mg.Movie.Name))
            }).ToList();
        }

        public CommandResponse Update(GenreRequest request)
        {
            var name = request?.Name?.Trim();

            if (string.IsNullOrWhiteSpace(name))
                return Error("Name is required!");

            if (Query().Any(g => g.Id != request.Id && g.Name == name))
                return Error("Genre with the same name exists!");

            var entity = Query(false).SingleOrDefault(g => g.Id == request.Id);
            if (entity is null)
                return Error("Genre not found!");

            entity.Name = name;

            Update(entity);
            return Success("Genre updated successfully.", entity.Id);
        }
    }
}
