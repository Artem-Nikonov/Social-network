using System.ComponentModel.DataAnnotations;

namespace SocialNetworkServer.Models
{
    public class UserRegistrationModel
    {
        [Required(ErrorMessage = "Введите имя")]
        [MinLength(2, ErrorMessage = "Длина имени должна быть не меньше 2-х символов.")]
        [RegularExpression(@"^[A-Za-zА-Яа-я]+$", ErrorMessage = "Имя должно содержать только буквы.")]
        [Display(Name = "Имя:")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите Фамилию")]
        [MinLength(2, ErrorMessage = "Длина фамилии должна быть не меньше 2-х символов.")]
        [RegularExpression(@"^[A-Za-zА-Яа-я]+$", ErrorMessage = "Фамилия должна содержать только буквы.")]
        [Display(Name = "Фамилия:")]
        public string UserSurname { get; set; }

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
