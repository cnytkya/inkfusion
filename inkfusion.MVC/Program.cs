using Microsoft.EntityFrameworkCore;
using inkfusion.MVC.Data;
using inkfusion.MVC.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// MySQL Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=localhost;Port=3307;Database=inkfusionDB;User=savascini;Password=Savascini123456;";

// Add Entity Framework Core with MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));

// Add session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add authentication
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.AccessDeniedPath = "/access-denied";
    });

var app = builder.Build();

// Migrate database and seed data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();

    if (!dbContext.Artists.Any())
    {
        dbContext.Artists.AddRange(
            new Artist { FirstName = "Yiğit", LastName = "Kırkyıldız", Specialization = "Realistik Dövmeler Uzmanı", Bio = "10+ yıl deneyime sahip", ExperienceYears = 10, InstagramUrl = "https://instagram.com", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Artist { FirstName = "Onur", LastName = "Sarı", Specialization = "Geleneksel Dövmeler Uzmanı", Bio = "Geleneksel stil uzmanı", ExperienceYears = 8, InstagramUrl = "https://instagram.com", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Artist { FirstName = "Ramazan", LastName = "Savaş Çinioğlu", Specialization = "Minimal Dövmeler Uzmanı", Bio = "Minimalist tasarımlar", ExperienceYears = 7, InstagramUrl = "https://instagram.com", IsActive = true, CreatedAt = DateTime.UtcNow }
        );
        dbContext.SaveChanges();
    }

    if (!dbContext.Services.Any())
    {
        dbContext.Services.AddRange(
            new Service { Name = "Özel Tasarım", Description = "Hayalinizdeki dövmeyi birlikte tasarlayalım.", Price = 500, DurationMinutes = 120, IconClass = "fas fa-drafting-compass", DisplayOrder = 1, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Service { Name = "Renkli Dövmeler", Description = "Canlı ve kalıcı renklerle tasarım.", Price = 600, DurationMinutes = 150, IconClass = "fas fa-paint-brush", DisplayOrder = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Service { Name = "Cover Up (Dövme Kapatma)", Description = "İstemediğiniz dövmeleri dönüştürüyoruz.", Price = 700, DurationMinutes = 180, IconClass = "fas fa-magic", DisplayOrder = 3, IsActive = true, CreatedAt = DateTime.UtcNow }
        );
        dbContext.SaveChanges();
    }

    if (!dbContext.Categories.Any())
    {
        dbContext.Categories.AddRange(
            new Category { Id = 1, Name = "Realistik", Description = "Realistik Dövmeler", Icon = "fas fa-image", DisplayOrder = 1, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Category { Id = 2, Name = "Geleneksel", Description = "Geleneksel Dövmeler", Icon = "fas fa-scroll", DisplayOrder = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Category { Id = 3, Name = "Minimal", Description = "Minimal Dövmeler", Icon = "fas fa-minimize", DisplayOrder = 3, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Category { Id = 4, Name = "Renkli", Description = "Renkli Dövmeler", Icon = "fas fa-palette", DisplayOrder = 4, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Category { Id = 5, Name = "Dermal & Piercingler", Description = "Dermal & Piercingler", Icon = "fas fa-gem", DisplayOrder = 5, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Category { Id = 6, Name = "Cover Up", Description = "Dövme Kapatma", Icon = "fas fa-magic", DisplayOrder = 6, IsActive = true, CreatedAt = DateTime.UtcNow }
        );
        dbContext.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
