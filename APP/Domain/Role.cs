using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Domain
{
    public class Role : Entity
    {
        [Required, StringLength(200)]
        public string Name { get; set; }

        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();

        [NotMapped]
        public List<int> UserIds
        {
            get => UserRoles.Select(userRoleEntity => userRoleEntity.UserId).ToList();
            set => UserRoles = value?.Select(userId => new UserRole() { UserId = userId }).ToList();
        }
    }
}