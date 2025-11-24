using APP.Domain;
using APP.Models;
using APP.Services;
using CORE.APP.Services;
using CORE.APP.Services.Authentication.MVC;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DbContext, Db>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Db")));

// Add Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
        options.AccessDeniedPath = "/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

builder.Services.AddScoped<IService<GroupRequest, GroupResponse>, GroupService>();
builder.Services.AddScoped<IService<RoleRequest, RoleResponse>, RoleService>();
builder.Services.AddScoped<IService<UserRequest, UserResponse>, UserService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICookieAuthService, CookieAuthService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<Db>();

        db.Database.EnsureCreated();

        var requiredRoles = new[] { "Admin", "User" };
        var existing = db.Roles.Select(r => r.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

        var toAdd = requiredRoles.Where(name => !existing.Contains(name))
                                 .Select(name => new Role { Name = name })
                                 .ToList();

        if (toAdd.Count > 0)
        {
            Console.WriteLine($"Seeding missing roles: {string.Join(", ", toAdd.Select(r => r.Name))}");
            db.Roles.AddRange(toAdd);
            db.SaveChanges();
        }

        Console.WriteLine("Roles in DB:");
        foreach (var role in db.Roles.OrderBy(r => r.Name))
            Console.WriteLine($"- {role.Name} (ID: {role.Id})");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during seeding: {ex.Message}");
        Console.WriteLine(ex.StackTrace);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
