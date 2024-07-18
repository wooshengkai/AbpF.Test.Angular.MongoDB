//using Microsoft.AspNetCore.Identity;
//using System.Threading.Tasks;
//using Volo.Abp;
//using Volo.Abp.Application.Services;
//using Volo.Abp.Identity;

//using Microsoft.AspNetCore.Identity;
using AbpF.Test.Angular.MongoDB.Members;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

//using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
//using Volo.Abp.Identity;

namespace AbpF.Test.Angular.MongoDB.Admins;

//public class AdminAppService :
//    CrudAppService<
//        AdminDB, //The Book entity
//        AdminDto, //Used to show books
//        Guid, //Primary key of the book entity
//        PagedAndSortedResultRequestDto, //Used for paging/sorting
//        CreateUpdateAdminDto>, //Used to create/update a book
//    IAdminAppService //implement the IBookAppService
public class AdminAppService : IAdminAppService
{
    //public AdminAppService(IRepository<AdminDB, Guid> repository)
    //    : base(repository)
    //{

    //}
    
    protected IAdminDBRepository AdminRepository { get; }

    public AdminAppService(
        IAdminDBRepository adminRepository
    )
    {
        AdminRepository = adminRepository;
    }

    public async Task<bool> Add()
    {
        var adminDto = await AdminRepository.Insert1Async();

        return adminDto;
    }

    public async Task<AdminDto> Get()
    {
        AdminDB adminDb = await AdminRepository.FindAsync();

        AdminDto? adminDto = null;

        if (adminDb != null)
        {
            adminDto = new AdminDto()
            {
                UserName = adminDb.UserName,
                Password = adminDb.Password,
                Email = adminDb.Email ?? "",
            };
        }

        return adminDto ?? new AdminDto();
    }
}

//public class AdminAppService : ApplicationService, IAdminAppService
//{
//    //private readonly SignInManager<Member> _signInManager;
//    //private readonly UserManager<Member> _userManager;
//    private readonly IConfiguration _configuration;

//    public AdminAppService(
//        //SignInManager<Member> signInManager,
//        //UserManager<Member> userManager,
//            IConfiguration configuration)
//    {
//        //_signInManager = signInManager;
//        //_userManager = userManager;
//        _configuration = configuration;
//    }

//    public async Task<AdminDto> Add()
//    {

//        return token;
//    }
//}