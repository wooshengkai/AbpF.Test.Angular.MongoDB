using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace AbpF.Test.Angular.MongoDB.Data;

/* This is used if database provider does't define
 * IMongoDBDbSchemaMigrator implementation.
 */
public class NullMongoDBDbSchemaMigrator : IMongoDBDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
