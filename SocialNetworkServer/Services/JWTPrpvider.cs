using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialNetworkServer.Interfaces;
using SocialNetworkServer.OptionModels;
using SocialNetworkServer.SocNetworkDBContext.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialNetworkServer.Services
{
    public class JWTPrpvider: IJWTProvider
    {
        public string GenerateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, $"{user.UserName} {user.UserSurname}")
            };
            var signingCredentials = new SigningCredentials(JWTOptions.GetSymmetricSecurityKey(),SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: JWTOptions.ISSUER,
                audience: JWTOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromHours(JWTOptions.ExpiresHours)),
                signingCredentials: signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
