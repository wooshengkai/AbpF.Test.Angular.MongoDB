using AbpF.Test.Angular.MongoDB.MongoDB;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AbpF.Test.Angular.MongoDB.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(MongoDBMongoDbModule),
    typeof(MongoDBApplicationContractsModule)
    )]
public class MongoDBDbMigratorModule : AbpModule
{
}
