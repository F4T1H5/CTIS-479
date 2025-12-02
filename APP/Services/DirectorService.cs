using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    public class DirectorService : Service<Director>, IService<DirectorRequest, DirectorResponse>
    {
        public DirectorService(DbContext db) : base(db)
        {
        }
        protected override IQueryable<Director> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).Include(d => d.Movies);
        }

        public CommandResponse Create(DirectorRequest request)
        {
            if (request is null)
                return Error("Request is null!");

            var firstName = request.FirstName?.Trim();
            var lastName = request.LastName?.Trim();

            if (string.IsNullOrWhiteSpace(firstName))
                return Error("First name is required!");
            if (string.IsNullOrWhiteSpace(lastName))
                return Error("Last name is required!");

            if (Query().Any(d => d.FirstName == firstName && d.LastName == lastName))
                return Error("Director with the same first name and last name exists!");

            var entity = new Director
            {
                FirstName = firstName,
                LastName = lastName,
                IsRetired = request.IsRetired
            };

            Create(entity);
            return Success("Director created successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false).SingleOrDefault(d => d.Id == id);
            if (entity is null)
                return Error("Director not found!");

            if (entity.Movies is not null && entity.Movies.Any())
            {
                var movieCount = entity.Movies.Count;
                return Error($"Director can't be deleted because it has {movieCount} relational movie(s). Please reassign or remove movies first.");
            }

            Delete(entity);
            return Success("Director deleted successfully.", entity.Id);
        }

        public DirectorRequest Edit(int id)
        {
            var entity = Query().SingleOrDefault(d => d.Id == id);
            if (entity is null)
                return null;

            return new DirectorRequest
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                IsRetired = entity.IsRetired
            };
        }

        public DirectorResponse Item(int id)
        {
            var entity = Query().SingleOrDefault(d => d.Id == id);
            if (entity is null)
                return null;

            return new DirectorResponse
            {
                Id = entity.Id,
                Guid = entity.Guid,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                IsRetired = entity.IsRetired
            };
        }

        public List<DirectorResponse> List()
        {
            return Query().Select(d => new DirectorResponse
            {
                Id = d.Id,
                Guid = d.Guid,
                FirstName = d.FirstName,
                LastName = d.LastName,
                IsRetired = d.IsRetired
            }).ToList();
        }

        public CommandResponse Update(DirectorRequest request)
        {
            if (request is null)
                return Error("Request is null!");

            var firstName = request.FirstName?.Trim();
            var lastName = request.LastName?.Trim();

            if (string.IsNullOrWhiteSpace(firstName))
                return Error("First name is required!");
            if (string.IsNullOrWhiteSpace(lastName))
                return Error("Last name is required!");

            if (Query().Any(d => d.Id != request.Id && d.FirstName == firstName && d.LastName == lastName))
                return Error("Director with the same first name and last name exists!");

            var entity = Query(false).SingleOrDefault(d => d.Id == request.Id);
            if (entity is null)
                return Error("Director not found!");

            entity.FirstName = firstName;
            entity.LastName = lastName;
            entity.IsRetired = request.IsRetired;

            Update(entity);
            return Success("Director updated successfully.", entity.Id);
        }
    }
}
