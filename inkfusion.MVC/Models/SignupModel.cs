using System.ComponentModel.DataAnnotations;

namespace inkfusion.MVC.Models
{
    public class SignupModel
    {
        [Required(ErrorMessage = "Ad gereklidir")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Ad 2-50 karakter arasında olmalıdır")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Soyadı gereklidir")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Soyadı 2-50 karakter arasında olmalıdır")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "E-posta adı gereklidir")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı gereklidir")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Kullanıcı adı 3-30 karakter arasında olmalıdır")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Şifreyi doğrulayın")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Şartları kabul etmelisiniz")]
        [Display(Name = "Hizmet Şartlarını ve Gizlilik Politikasını Kabul Ediyorum")]
        public bool AcceptTerms { get; set; }
    }
}
