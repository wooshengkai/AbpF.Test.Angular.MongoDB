using Volo.Abp.Modularity;

namespace AbpF.Test.Angular.MongoDB;

[DependsOn(
    typeof(MongoDBDomainModule),
    typeof(MongoDBTestBaseModule)
)]
public class MongoDBDomainTestModule : AbpModule
{

}
