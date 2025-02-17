using System.ComponentModel.DataAnnotations;
using LezzetKapinda.Models;
using LezzetKapinda.ValueObjects;

namespace LezzetKapinda.ViewModels;

public sealed class LoginViewModel
{
        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        [Display(Name = "Kullanıcı Adı")]
        public UserName UserName { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public Password Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }

        public User User { get; set; }
}