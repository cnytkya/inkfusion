using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inkfusion.MVC.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }

        [Required]
        [ForeignKey("Artist")]
        public int ArtistId { get; set; }

        [Required]
        [ForeignKey("Service")]
        public int ServiceId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [StringLength(50)]
        public string? TimeSlot { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? GuestName { get; set; }

        [StringLength(100)]
        public string? GuestEmail { get; set; }

        [StringLength(20)]
        public string? GuestPhone { get; set; }

        [Required]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; }

        public decimal? Price { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public User? User { get; set; }
        public Artist? Artist { get; set; }
        public Service? Service { get; set; }
    }

    public enum AppointmentStatus
    {
        Pending = 0,
        Confirmed = 1,
        InProgress = 2,
        Completed = 3,
        Cancelled = 4
    }
}
