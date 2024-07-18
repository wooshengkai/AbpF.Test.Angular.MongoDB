using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AbpF.Test.Angular.MongoDB.Admins;
using AbpF.Test.Angular.MongoDB.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace AbpF.Test.Angular.MongoDB.Admins;

public class MongoAdminDBRepository :
    MongoDbRepository<MongoDBMongoDbContext, AdminDB, Guid>,
    IAdminDBRepository
{
    public MongoAdminDBRepository(IMongoDbContextProvider<MongoDBMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<AdminDB> FindAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        return await (await GetMongoQueryableAsync(cancellationToken))
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async  Task<bool> Insert1Async()
    {
        //cancellationToken = GetCancellationToken(cancellationToken);

        var asd = new AdminDB()
        {
            UserName = "ased",

            Password = "qweqwe",

            Email = "qweqwe"
        };

        var zxc = await InsertAsync(asd);
        var qwe = "asd";

        return true;
    }
}