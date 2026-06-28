using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inkfusion.MVC.Models
{
    public class GalleryItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public string? ImageUrl { get; set; }

        [ForeignKey("Artist")]
        public int? ArtistId { get; set; }

        [ForeignKey("Category")]
        public int? CategoryId { get; set; }

        [StringLength(50)]
        public string? Category { get; set; } // Keep for backwards compatibility

        [Range(1, 100)]
        public int DisplayOrder { get; set; } = 1;

        [Required]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Artist? Artist { get; set; }
        public Category? CategoryNavigation { get; set; }
    }
}
