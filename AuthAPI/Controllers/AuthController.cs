using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController()
        {
        }

        [HttpPost]
        [Route("sign-in-customer")]
        public AuthToken SigninCustomer([FromBody] User user)
        {
            var time = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                notBefore: time, 
                expires: time.Add(TimeSpan.FromMinutes(2)), 
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes("thisismysecretkeyforthisapigatewayproject")), SecurityAlgorithms.HmacSha256));
            var a = new AuthToken();
            a.AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
            a.Expires = TimeSpan.FromSeconds(30).TotalSeconds;
            return a;
        }
        
        [HttpPost]
        [Route("sign-in-product")]
        public AuthToken SigninProduct([FromBody] User user)
        {
            var time = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                notBefore: time, 
                expires: time.Add(TimeSpan.FromMinutes(2)), 
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes("thisismysecretkeyforthisapigatewayproject")), SecurityAlgorithms.HmacSha256));
            var a = new AuthToken();
            a.AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
            a.Expires = TimeSpan.FromSeconds(30).TotalSeconds;
            return a;
        }
    }
}
