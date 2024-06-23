using System.Threading.Tasks;

namespace AbpF.Test.Angular.MongoDB.Data;

public interface IMongoDBDbSchemaMigrator
{
    Task MigrateAsync();
}
