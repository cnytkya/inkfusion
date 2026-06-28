namespace inkfusion.MVC.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        // Relationships
        public ICollection<GalleryItem> GalleryItems { get; set; } = new List<GalleryItem>();
    }
}
