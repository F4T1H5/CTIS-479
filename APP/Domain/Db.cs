using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics.Metrics;

namespace APP.Domain
{
    public class Db : DbContext
    {
        public DbSet<Group> Groups { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public Db(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>().HasIndex(groupEntity => groupEntity.Title).IsUnique();

            modelBuilder.Entity<Role>().HasIndex(roleEntity => roleEntity.Name).IsUnique();

            modelBuilder.Entity<User>().HasIndex(userEntity => userEntity.UserName).IsUnique();

            modelBuilder.Entity<User>().HasIndex(userEntity => new { userEntity.FirstName, userEntity.LastName });

            modelBuilder.Entity<UserRole>()
                .HasOne(userRoleEntity => userRoleEntity.User) // each UserRole entity has one related User entity
                .WithMany(userEntity => userEntity.UserRoles) // each User entity has many related UserRole entities
                .HasForeignKey(userRoleEntity => userRoleEntity.UserId) // the foreign key property in the UserRole entity that
                                                                        // references the primary key of the related User entity
                .OnDelete(DeleteBehavior.NoAction); // prevents deletion of a User entity if there are related UserRole entities

            modelBuilder.Entity<UserRole>()
                .HasOne(userRoleEntity => userRoleEntity.Role) // each UserRole entity has one related Role entity
                .WithMany(roleEntity => roleEntity.UserRoles) // each Role entity has many related UserRole entities
                .HasForeignKey(userRoleEntity => userRoleEntity.RoleId) // the foreign key property in the UserRole entity that
                                                                        // references the primary key of the related Role entity
                .OnDelete(DeleteBehavior.NoAction); // prevents deletion of a Role entity if there are related UserRole entities

            modelBuilder.Entity<User>()
                .HasOne(userEntity => userEntity.Country) // each User entity has one related Country entity
                .WithMany(countryEntity => countryEntity.Users) // each Country entity has many related User entities
                .HasForeignKey(userEntity => userEntity.CountryId) // the foreign key property in the User entity that
                                                                   // references the primary key of the related Country entity
                .OnDelete(DeleteBehavior.NoAction); // prevents deletion of a Country entity if there are related User entities

            modelBuilder.Entity<User>()
                .HasOne(userEntity => userEntity.City) // each User entity has one related City entity
                .WithMany(cityEntity => cityEntity.Users) // each City entity has many related User entities
                .HasForeignKey(userEntity => userEntity.CityId) // the foreign key property in the User entity that
                                                                // references the primary key of the related City entity
                .OnDelete(DeleteBehavior.NoAction); // prevents deletion of a City entity if there are related User entities

            modelBuilder.Entity<User>()
                .HasOne(userEntity => userEntity.Group) // each User entity has one related Group entity
                .WithMany(groupEntity => groupEntity.Users) // each Group entity has many related User entities
                .HasForeignKey(userEntity => userEntity.GroupId) // the foreign key property in the User entity that
                                                                 // references the primary key of the related Group entity
                .OnDelete(DeleteBehavior.NoAction); // prevents deletion of a Group entity if there are related User entities
        }


    }
}
