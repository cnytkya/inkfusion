using System.ComponentModel.DataAnnotations;

namespace inkfusion.MVC.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Range(0, 100000)]
        public decimal Price { get; set; }

        [Range(15, 480)]
        public int DurationMinutes { get; set; } = 60;

        public string? IconClass { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Range(1, 5)]
        public int DisplayOrder { get; set; } = 1;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Appointment>? Appointments { get; set; }
    }
}
