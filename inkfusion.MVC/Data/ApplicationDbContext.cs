using Microsoft.EntityFrameworkCore;
using inkfusion.MVC.Models;

namespace inkfusion.MVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<GalleryItem> GalleryItems { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed admin user
            var seedDate = new DateTime(2026, 6, 28, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Savas",
                    LastName = "Ciniogli",
                    Email = "savas.ciniogli@gmail.com",
                    Username = "savascini7",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Ciniogli12345"),
                    Role = UserRole.Admin,
                    IsActive = true,
                    CreatedAt = seedDate
                }
            );

            // Seed categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Realistik Dövmeler", Description = "Gerçeğe yakın, detaylı dövmeler", Icon = "fas fa-eye", DisplayOrder = 1, IsActive = true, CreatedAt = seedDate },
                new Category { Id = 2, Name = "Dermal & Piercingler", Description = "Dermal implantlar ve vücut piercingleri", Icon = "fas fa-gem", DisplayOrder = 2, IsActive = true, CreatedAt = seedDate },
                new Category { Id = 3, Name = "Çizgisel ve Yazı", Description = "Minimalist çizgisel ve anlamlı yazı dövmeleri", Icon = "fas fa-pen-nib", DisplayOrder = 3, IsActive = true, CreatedAt = seedDate },
                new Category { Id = 4, Name = "Renkli", Description = "Renkli ve canlı dövmeler", Icon = "fas fa-palette", DisplayOrder = 4, IsActive = true, CreatedAt = seedDate },
                new Category { Id = 5, Name = "Geometrik", Description = "Geometrik şekiller ve desenler", Icon = "fas fa-shapes", DisplayOrder = 5, IsActive = true, CreatedAt = seedDate },
                new Category { Id = 6, Name = "Geleneksel", Description = "Geleneksel tatto stilleri", Icon = "fas fa-scroll", DisplayOrder = 6, IsActive = true, CreatedAt = seedDate }
            );

            // Seed artists
            modelBuilder.Entity<Artist>().HasData(
                new Artist
                {
                    Id = 1,
                    FirstName = "Yiğit",
                    LastName = "Kırkyıldız",
                    Specialization = "Realistik Dövmeler Uzmanı",
                    Bio = "10+ yıl deneyime sahip realistik dövme uzmanı",
                    ExperienceYears = 10,
                    InstagramUrl = "https://instagram.com",
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new Artist
                {
                    Id = 2,
                    FirstName = "Onur",
                    LastName = "Sarı",
                    Specialization = "Geleneksel Dövmeler Uzmanı",
                    Bio = "Geleneksel stil dövmelerde uzman",
                    ExperienceYears = 8,
                    InstagramUrl = "https://instagram.com",
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new Artist
                {
                    Id = 3,
                    FirstName = "Ramazan",
                    LastName = "Savaş Çinioğlu",
                    Specialization = "Minimal Dövmeler Uzmanı",
                    Bio = "Minimalist ve zarif tasarımlar",
                    ExperienceYears = 7,
                    InstagramUrl = "https://instagram.com",
                    IsActive = true,
                    CreatedAt = seedDate
                }
            );

            // Relationships
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.User)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Artist)
                .WithMany(ar => ar.Appointments)
                .HasForeignKey(a => a.ArtistId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
