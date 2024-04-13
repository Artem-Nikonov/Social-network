using SocialNetworkServer.SocNetworkDBContext.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SocialNetworkServer.Enums;

namespace SocialNetworkServer.Models
{
    public class GroupInfoModel
    {
        [BindNever]
        public int GroupId { get; set; }
        [BindNever]
        public int CreatorId { get; set; }
        [Required(ErrorMessage ="Введите название группы")]
        [MinLength(2, ErrorMessage = "Длина названия должна быть не меньше 2-х символов.")]
        [Display(Name ="Название группы")]
        public string GroupName { get; set; }
        [Required(ErrorMessage = "Введите описание группы")]
        [MinLength(2, ErrorMessage = "Длина описания должна быть не меньше 2-х символов.")]
        [Display(Name = "Описание группы")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Выберите, кто сможет публиковать посты")]
        [Display(Name = "Разрешение на публикацию постов")]
        public PostPermissions PostPermissions { get; set; }

        public static implicit operator GroupInfoModel(Group group)
        {
            return new GroupInfoModel
            {
                GroupId = group.GroupId,
                CreatorId = group.CreatorId,
                GroupName = group.GroupName,
                Description = group.Description,
                PostPermissions = group.PostPermissions
            };
        }
    }
}
