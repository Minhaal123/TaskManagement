using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BackendNet5WebAPI.ExtensionMethods
{
    public class Authenticate
    {
        private  readonly IConfiguration _configuration;

       // private readonly IConfiguration _configuration;
        public Authenticate(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string GenerateJSONWebToken(string username, List<Claim> authclaims)
        {
            //KEY
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            //ALGO
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //CLAIMS
            var claims = authclaims;

            //var claims = new[] {
            //                    new Claim("Issuer","Sheikh"),
            //                    new Claim("Admin","true"),
            //                    new Claim(JwtRegisteredClaimNames.UniqueName,username)};

            var token = new JwtSecurityToken(
                 //Issuer who created it
                 _configuration["JWT:ValidIssuer"],
                 //for whome this is created
                 _configuration["JWT:ValidAudience"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
                );







            return new JwtSecurityTokenHandler().WriteToken(token);
        }


      
    }
}
