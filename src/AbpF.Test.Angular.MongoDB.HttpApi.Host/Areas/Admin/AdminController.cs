﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Volo.Abp.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using AbpF.Test.Angular.MongoDB.Admins;

namespace AbpF.Test.Angular.MongoDB.Areas.Admin.Controllers
{
    [Controller]
    [Area("admin")]
    [Route("[area]/[controller]")]
    public class AdminController : AbpController
    {
        private readonly IConfiguration _configuration;
        private readonly IAdminAppService _adminAppService;

        public AdminController(IConfiguration configuration, IAdminAppService adminAppService)
        {
            _configuration = configuration;
            _adminAppService = adminAppService;
        }

        [HttpGet]
        [Route("TestAbc")]
        public string Test()
        {
            //return "asd3";

            var asd = _adminAppService.Add();
            var zxc = _adminAppService.Get();

            Claim[] claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, /*user.Id.ToString()*/"123"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
