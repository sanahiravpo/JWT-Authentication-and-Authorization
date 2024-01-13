using JWTAuthentification_new.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace JWTAuthentification_new.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogerController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public LogerController(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        [Authorize]
        [HttpGet]
        [Route(" GetunAuthorised")]
        public ActionResult GetunAuthorised()
        {

            return Ok("hello from unAuthorised");
        }
        [Authorize]
        [HttpPost]
        [Route("GetAuthorised")]
        public  ActionResult GetAuthorised()
        {
            return Ok("hello from Authorised");
        }
        [Route(" CkeckLogin")]
        [AllowAnonymous]
        [HttpPost]
        
        public async Task<ActionResult> CkeckLogin( userLogin model)
        {
            if (model.Loginid == "admin" && model.password == "password")
            {
                string token = GenerateJwtToken(model);
                return Ok(new { Token = token });
            }
            return Ok();
            
        }

        private string GenerateJwtToken(userLogin model)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddHours(1);

            var token = new JwtSecurityToken(
                //issuer: _configuration["Jwt:Issuer"],
                //audience: _configuration["Jwt:Audience"],
                claims: new[] {
                    new Claim(ClaimTypes.Name, model.Loginid),
                    new Claim(ClaimTypes.NameIdentifier, model.Loginid)
                },
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
