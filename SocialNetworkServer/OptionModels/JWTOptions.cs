using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SocialNetworkServer.OptionModels
{
    public class JWTOptions
    {
        public const string ISSUER = "MyAuthServer"; // издатель токена
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена
        const string KEY = "SecretKeySocialNetworkJWTAuthantikationTotalCringeStudioSecure";
        public const int ExpiresHours = 5;
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
