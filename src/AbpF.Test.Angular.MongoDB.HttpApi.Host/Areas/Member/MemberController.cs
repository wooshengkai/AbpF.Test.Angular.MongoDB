using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using AbpF.Test.Angular.MongoDB.Members;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Microsoft.Extensions.Configuration;
//using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
//using Castle.Core.Configuration;

namespace AbpF.Test.Angular.MongoDB.Areas.Member.Controllers
{
    [Controller]
    [Area("member")]
    [Route("[area]/[controller]")]
    public class MemberController : ControllerBase1
    {
        private readonly IMemberAppService _memberAppService;
        private readonly IConfiguration _configuration;

        public MemberController(IMemberAppService memberAppService, IConfiguration configuration)
        {
            _memberAppService = memberAppService;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("test")]
        public string Test()
        {
            return "asd";
        }

        [HttpPost]
        [Route("login")]
        public async Task<string> LoginAsync([FromBody]MemberDto input)
        {
            return await _memberAppService.LoginAsync(input);
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("login1")]
        public string Login([FromBody] MemberDto input)
        {
            var asd = CurrUser?.UserId;
            HttpContext.Items["code"] = 200;
            HttpContext.Items["message"] = "Login successful " + asd.ToString();
            var token = GenerateJwtToken(input.UserName);
            return token;
        }

        private string GenerateJwtToken(string username)
        {
            string asd = _configuration["Jwt:Key"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(asd));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}