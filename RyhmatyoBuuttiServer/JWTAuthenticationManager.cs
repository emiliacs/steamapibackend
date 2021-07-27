using Microsoft.IdentityModel.Tokens;
using RyhmatyoBuuttiServer.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RyhmatyoBuuttiServer
{
    public interface IJWTAuthenticationManager
    {
        string generateJWT(User user);
    }
    public class JWTAuthenticationManager : IJWTAuthenticationManager
    {

        private readonly string tokenKey;

        public JWTAuthenticationManager(string tokenKey)
        {
            this.tokenKey = tokenKey;
        }

        public string generateJWT(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
