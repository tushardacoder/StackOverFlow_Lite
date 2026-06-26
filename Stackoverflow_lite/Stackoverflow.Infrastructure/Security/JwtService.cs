using Microsoft.IdentityModel.Tokens;
using Stackoverflow.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Stackoverflow.Infrastructure.Security
{
    public class JwtService : IJwtService
    {

        public string GenerateToken(ApplicationUser user)
        {


            var claims = new[]
          {
              new Claim(
                  "userid",
                 user.Id.ToString()),

              new Claim(
              "email",
              user.Email)
            };


            var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
            "secret_key_secret_key_12345_secret_key_12345_secret_key_12345_secret_key_12345"));


            var token = new JwtSecurityToken(

            claims: claims,

            expires:
            DateTime.UtcNow.AddHours(1),

            signingCredentials:
            new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256)

            );



            return new JwtSecurityTokenHandler()
            .WriteToken(token);

        }

    }
}
