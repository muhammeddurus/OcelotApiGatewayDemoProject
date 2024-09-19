using System.ComponentModel.DataAnnotations;

namespace AuthServer.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Lütfen kullanıcı adınızı giriniz.")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Lütfen şifrenizi giriniz.")]
        public string Password { get; set; }
    }
}
