using BackendNet5WebAPI.Authentication;
using BackendNet5WebAPI.Enum;
using BackendNet5WebAPI.ExtensionMethods;
using BackendNet5WebAPI.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BackendNet5WebAPI.Controllers
{
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> roleManager;

        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            _configuration = configuration;
            this.roleManager = roleManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                Authenticate authenticate = new Authenticate(_configuration);
                var user = await userManager.FindByNameAsync(model.username);
                if (user != null && await userManager.CheckPasswordAsync(user, model.password))
                {
                    var userRoles = await userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("Admin","true"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }


                   var token= authenticate.GenerateJSONWebToken(ClaimTypes.Name,authClaims);


                    return Ok(new
                    {
                        statusCode = 200,
                        token = token                     
                    });
                }
                return Ok(new
                {
                    statusCode = 401,
                    statusMessage = "You are not authorized user!"
                });
                //return Unauthorized();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

      


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return Ok(new
                {
                    statusCode = 401,
                    statusMessage = "User already exists!"
                });
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return Ok(new
                {
                    statusCode = 401,
                    statusMessage = "User creation failed! Please check user details and try again!"
                });
            }




            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

            if (await roleManager.RoleExistsAsync("Admin"))
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }



            RegisterUser registeredUser = new RegisterUser();
            registeredUser.id = user.Id;
            registeredUser.email = user.Email;
            registeredUser.created_at = DateTime.UtcNow;
            registeredUser.updated_at = DateTime.UtcNow.AddDays(1);
            return Ok(new
            {
                statusCode = 200,
                statusMessage = "User has been successfully registered.",
                user = registeredUser
            });
        }

    }
}
