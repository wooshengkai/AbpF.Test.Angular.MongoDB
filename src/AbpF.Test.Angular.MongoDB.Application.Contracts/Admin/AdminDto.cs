using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace AbpF.Test.Angular.MongoDB.Admins;

public class AdminDto : AuditedEntityDto<Guid>
{
    public string UserName { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }
}

public class CreateUpdateAdminDto
{
    [Required]
    //[StringLength(128)]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Email { get; set; }
}