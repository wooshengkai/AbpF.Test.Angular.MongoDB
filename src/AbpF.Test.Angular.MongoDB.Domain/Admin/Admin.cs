using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpF.Test.Angular.MongoDB.Admins;

//public class AdminDB : FullAuditedAggregateRoot<Guid>
public class AdminDB : FullAuditedAggregateRoot<Guid>
{
    public required string UserName { get; set; }

    public required string Password { get; set; }

    public string? Email { get; set; }
}