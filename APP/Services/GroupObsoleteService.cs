using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;

namespace APP.Services
{
    [Obsolete("Use GroupService class instead!")]
    public class GroupObsoleteService : ServiceBase
    {
        private readonly Db _db;

        public GroupObsoleteService(Db db)
        {
            _db = db;
        }

        public IQueryable<GroupResponse> Query()
        {
            var query = _db.Groups.Select(groupEntity => new GroupResponse()
            {
                Id = groupEntity.Id,
                Guid = groupEntity.Guid,
                Title = groupEntity.Title,
            });

            return query;
        }

        public GroupRequest Edit(int id)
        {
            var entity = _db.Groups.Find(id);

            if (entity is null)
                return null;

            var request = new GroupRequest()
            {
                Id = entity.Id,
                Title = entity.Title,
            };

            return request;
        }

        public CommandResponse Create(GroupRequest request)
        {
            if (_db.Groups.Any(groupEntity => groupEntity.Title == request.Title.Trim()))
                return Error("Group with the same title exists!");

            var entity = new Group()
            {
                Guid = Guid.NewGuid().ToString(),
                Title = request.Title.Trim(),
            };

            _db.Groups.Add(entity);

            _db.SaveChanges();

            return Success("Group created successfully.", entity.Id);
        }

        public CommandResponse Update(GroupRequest request)
        {
            if (_db.Groups.Any(groupEntity => groupEntity.Id != request.Id && groupEntity.Title == request.Title.Trim()))
                return Error("Group with the same title exists!");

            var entity = _db.Groups.Find(request.Id);
            if (entity is null)
                return Error("Group not found!");

            entity.Title = request.Title?.Trim();

            _db.Groups.Update(entity);

            _db.SaveChanges();

            return Success("Group updated successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = _db.Groups.Find(id);

            if (entity is null)
                return Error("Group not found!");

            _db.Groups.Remove(entity);

            _db.SaveChanges();

            return Success("Group deleted successfully.", entity.Id);
        }
    }
}
