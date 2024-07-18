using System;
using Volo.Abp.Application.Dtos;

namespace AbpF.Test.Angular.MongoDB.Members;

public class MemberDto
{
    public string UserName { get; set; }

    public string Password { get; set; }

    //public string Email { get; set; }
}

public class UserInfoModel
{
    public string UserId { get; set; }

    public string UserName { get; set; }
}