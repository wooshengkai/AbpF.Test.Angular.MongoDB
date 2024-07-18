using System.Collections.Generic;
using System.Threading;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace AbpF.Test.Angular.MongoDB.Admins;

public interface IAdminDBRepository : IBasicRepository<AdminDB, Guid>
{
    Task<AdminDB> FindAsync(CancellationToken cancellationToken = default);

    Task<bool> Insert1Async();
}