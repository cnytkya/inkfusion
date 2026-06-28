using System.ComponentModel.DataAnnotations;

namespace inkfusion.MVC.Models
{
    public class Artist
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string? LastName { get; set; }

        [StringLength(500)]
        public string? Bio { get; set; }

        [StringLength(500)]
        public string? Specialization { get; set; }

        public string? ImageUrl { get; set; }

        [Range(1, 50)]
        public int ExperienceYears { get; set; }

        [StringLength(500)]
        public string? InstagramUrl { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Appointment>? Appointments { get; set; }
    }
}
