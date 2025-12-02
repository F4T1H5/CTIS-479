using APP.Domain;
using APP.Models;
using Microsoft.EntityFrameworkCore;

namespace APP.Data;

public static class DbInitializer
{
    public static void Initialize(Db context)
    {
        // Apply any pending migrations automatically
        context.Database.Migrate();

        // Seed roles if they don't exist
        SeedRoles(context);
        
        // Optional: Seed initial data
        SeedInitialData(context);
    }

    private static void SeedRoles(Db context)
    {
        var requiredRoles = new[] { "Admin", "User" };
        var existing = context.Roles.Select(r => r.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

        var toAdd = requiredRoles
            .Where(name => !existing.Contains(name))
            .Select(name => new Role { Name = name })
            .ToList();

        if (toAdd.Count > 0)
        {
            Console.WriteLine($"Seeding missing roles: {string.Join(", ", toAdd.Select(r => r.Name))}");
            context.Roles.AddRange(toAdd);
            context.SaveChanges();
        }

        Console.WriteLine("Roles in DB:");
        foreach (var role in context.Roles.OrderBy(r => r.Name))
        {
            Console.WriteLine($"- {role.Name} (ID: {role.Id})");
        }
    }

    private static void SeedInitialData(Db context)
    {
        // Seed sample genres if empty
        if (!context.Genres.Any())
        {
            context.Genres.AddRange(
                new Genre { Name = "Action" },
                new Genre { Name = "Comedy" },
                new Genre { Name = "Drama" },
                new Genre { Name = "Horror" },
                new Genre { Name = "Science Fiction" }
            );
            context.SaveChanges();
        }

        // Seed sample directors if empty
        if (!context.Directors.Any())
        {
            context.Directors.AddRange(
                new Director { FirstName = "Christopher", LastName = "Nolan", IsRetired = false },
                new Director { FirstName = "Steven", LastName = "Spielberg", IsRetired = false },
                new Director { FirstName = "Martin", LastName = "Scorsese", IsRetired = false }
            );
            context.SaveChanges();
        }
    }
}