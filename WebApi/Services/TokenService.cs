using System.Security.Claims;
using System.Text;
using WebApi.Model;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Services
{
    public static class TokenService
    {
 
        public static string GenerateToken(IConfiguration _configuration, UserRegister user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(10),
                Subject = new ClaimsIdentity(new Claim[] 
          {
             new Claim(ClaimTypes.Name, user.Name)
          }),
               
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Settings.JwtKey)), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
