using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetworkServer.SocNetworkDBContext.Entities
{

    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserSurname { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string FormattedRegistrationDate { get; set; }

        public List<Post> Posts { get; set; } = new();
    }
}
