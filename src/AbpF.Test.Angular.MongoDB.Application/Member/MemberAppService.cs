//using Microsoft.AspNetCore.Identity;
//using System.Threading.Tasks;
//using Volo.Abp;
//using Volo.Abp.Application.Services;
//using Volo.Abp.Identity;

//using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
//using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
//using Volo.Abp.Identity;

namespace AbpF.Test.Angular.MongoDB.Members;

public class MemberAppService : ApplicationService, IMemberAppService
{
    //private readonly SignInManager<Member> _signInManager;
    //private readonly UserManager<Member> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IDistributedCache<MemberDto> _cache;

    public MemberAppService(
        //SignInManager<Member> signInManager,
        //UserManager<Member> userManager,
        IDistributedCache<MemberDto> cache,
            IConfiguration configuration)
    {
        //_signInManager = signInManager;
        //_userManager = userManager;
        _configuration = configuration;
        _cache = cache;
    }

    public async Task<string> LoginAsync(MemberDto input)
    {
        //Member user = await _userManager.FindByNameAsync(input.UserName);
        //if (user == null)
        //{
        //    throw new UserFriendlyException("User not found");
        //}
        Member user = new()
        {
            UserName = "test"
            ,
            Password = "test"
            ,
            Email = "test"
        };

        //var result = await _signInManager.PasswordSignInAsync(user, input.Password, false, true);
        //if (!result.Succeeded)
        //{
        //    throw new UserFriendlyException("Invalid login attempt");
        //}

        //void Set(TCacheKey key, TCacheItem value, DistributedCacheEntryOptions? options = null, bool? hideErrors = null, bool considerUow = false);
        _cache.SetAsync("te1", new MemberDto() { UserName = "123", Password = "123" });

        var zxc = await _cache.GetAsync("te1");

        var token = GenerateJwtToken(user);

        return token;
    }

    private string GenerateJwtToken(Member user)
    {
        try
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, /*user.Id.ToString()*/"123456789"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            string asd = _configuration["Jwt:Key"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(asd));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                //issuer: _configuration["Jwt:Issuer"],
                //audience: _configuration["Jwt:Audience"],
                claims: claims,
                //expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
            //return "";
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }
}