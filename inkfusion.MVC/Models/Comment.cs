using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inkfusion.MVC.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [StringLength(500)]
        public string? Text { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; } = 5;

        [Required]
        public bool IsApproved { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ApprovedAt { get; set; }

        public User? User { get; set; }
    }
}
