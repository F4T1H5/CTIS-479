using APP.Domain;
using APP.Models;
using Microsoft.EntityFrameworkCore;

namespace APP.Data;

public static class DbInitializer
{
    public static void Initialize(Db context)
    {
        // Ensure database is created
        context.Database.EnsureCreated();

        // Seed roles if they don't exist
        SeedRoles(context);
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
}