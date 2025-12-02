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

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }

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


            // =========================
            // MOVIES
            // =========================

            modelBuilder.Entity<Genre>()
                .HasIndex(genreEntity => genreEntity.Name)
                .IsUnique();

            modelBuilder.Entity<Director>()
                .HasIndex(directorEntity => new { directorEntity.FirstName, directorEntity.LastName });

            modelBuilder.Entity<Movie>()
                .HasIndex(movieEntity => movieEntity.Name)
                .IsUnique(); // remove if movies with same name are allowed

            modelBuilder.Entity<Movie>()
                .Property(movieEntity => movieEntity.TotalRevenue)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Movie>()
                .HasOne(movieEntity => movieEntity.Director)          // each Movie has one Director
                .WithMany(directorEntity => directorEntity.Movies)    // each Director has many Movies
                .HasForeignKey(movieEntity => movieEntity.DirectorId)
                .OnDelete(DeleteBehavior.NoAction);

            // Many-to-many via join table: MovieGenre(MovieId, GenreId)
            modelBuilder.Entity<MovieGenre>()
                .HasKey(movieGenreEntity => new { movieGenreEntity.MovieId, movieGenreEntity.GenreId });

            modelBuilder.Entity<MovieGenre>()
                .HasOne(movieGenreEntity => movieGenreEntity.Movie)
                .WithMany(movieEntity => movieEntity.MovieGenres)
                .HasForeignKey(movieGenreEntity => movieGenreEntity.MovieId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MovieGenre>()
                .HasOne(movieGenreEntity => movieGenreEntity.Genre)
                .WithMany(genreEntity => genreEntity.MovieGenres)
                .HasForeignKey(movieGenreEntity => movieGenreEntity.GenreId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
