using System.ComponentModel.DataAnnotations;

namespace inkfusion.MVC.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string? Username { get; set; }

        [Required]
        public string? PasswordHash { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        [StringLength(500)]
        public string? Bio { get; set; }

        public string? ProfileImageUrl { get; set; }

        [Required]
        public UserRole Role { get; set; } = UserRole.Customer;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginAt { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Appointment>? Appointments { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }

    public enum UserRole
    {
        Customer = 0,
        Admin = 1
    }
}
