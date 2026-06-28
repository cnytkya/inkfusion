using inkfusion.MVC.Data;
using inkfusion.MVC.Models;

namespace inkfusion.MVC.Services
{
    public class ImageImportService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _galleryPath;

        public ImageImportService(ApplicationDbContext context, string webRootPath)
        {
            _context = context;
            _galleryPath = Path.Combine(webRootPath, "img", "gallery");
        }

        public async Task ImportImagesFromDirectoriesAsync()
        {
            try
            {
                var categoryMap = new Dictionary<string, int>
                {
                    { "cizgiselveyazi", 3 },           // Çizgisel ve Yazı
                    { "dermalvepiercingler", 2 },      // Dermal & Piercingler
                    { "realistic", 1 }                  // Realistik Dövmeler
                };

                var importedCount = 0;

                foreach (var categoryDir in categoryMap)
                {
                    var dirName = categoryDir.Key;
                    var categoryId = categoryDir.Value;
                    var dirPath = Path.Combine(_galleryPath, dirName);

                    if (!Directory.Exists(dirPath))
                    {
                        Console.WriteLine($"❌ Klasör bulunamadı: {dirPath}");
                        continue;
                    }

                    var images = Directory.GetFiles(dirPath, "*.jpg")
                        .Concat(Directory.GetFiles(dirPath, "*.png"))
                        .Concat(Directory.GetFiles(dirPath, "*.jpeg"))
                        .OrderBy(x => x)
                        .ToList();

                    Console.WriteLine($"📂 {dirName}: {images.Count} resim bulundu");

                    foreach (var imagePath in images)
                    {
                        var fileName = Path.GetFileName(imagePath);
                        var relativePath = $"/img/gallery/{dirName}/{fileName}";

                        // Zaten varsa atla
                        if (_context.GalleryItems.Any(g => g.ImageUrl == relativePath))
                        {
                            continue;
                        }

                        var galleryItem = new GalleryItem
                        {
                            Title = Path.GetFileNameWithoutExtension(fileName),
                            Description = $"Resim: {fileName}",
                            ImageUrl = relativePath,
                            CategoryId = categoryId,
                            DisplayOrder = images.IndexOf(imagePath) + 1,
                            IsActive = true,
                            CreatedAt = DateTime.UtcNow
                        };

                        _context.GalleryItems.Add(galleryItem);
                        importedCount++;
                    }
                }

                await _context.SaveChangesAsync();
                Console.WriteLine($"✅ Toplam {importedCount} resim DB'ye yazıldı!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Hata: {ex.Message}");
            }
        }
    }
}
