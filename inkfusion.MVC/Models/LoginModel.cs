using System.ComponentModel.DataAnnotations;

namespace inkfusion.MVC.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "E-posta adı gereklidir")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}
