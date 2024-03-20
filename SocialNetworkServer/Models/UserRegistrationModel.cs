using System.ComponentModel.DataAnnotations;

namespace SocialNetworkServer.Models
{
    public class UserRegistrationModel
    {
        [Display(Name ="Логин:")]
        [Required(ErrorMessage = "Введите логин")]
        [StringLength(12, MinimumLength =5, ErrorMessage ="Длина логина должна быть от 5 до 12 символов.")]
        public string Login { get; set; }

        [Display(Name = "Пароль:")]
        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Длина пароля должна быть от 4 до 30 символов.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Подтвердите пароль:")]
        [Required(ErrorMessage = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
