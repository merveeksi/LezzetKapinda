using System.ComponentModel.DataAnnotations;

namespace LezzetKapinda.ViewModels;

public sealed class ContactViewModel
{
       [Required(ErrorMessage = "Lütfen adınızı ve soyadınızı giriniz.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Lütfen e-posta adresinizi giriniz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lütfen konu başlığını giriniz.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Lütfen mesajınızı giriniz.")]
        public string Message { get; set; }
}