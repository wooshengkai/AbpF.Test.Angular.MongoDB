using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpF.Test.Angular.MongoDB.Members;

public class Member : FullAuditedAggregateRoot<Guid>
{
    public string UserName { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }
}