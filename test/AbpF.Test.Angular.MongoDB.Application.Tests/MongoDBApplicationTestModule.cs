using Volo.Abp.Modularity;

namespace AbpF.Test.Angular.MongoDB;

[DependsOn(
    typeof(MongoDBApplicationModule),
    typeof(MongoDBDomainTestModule)
)]
public class MongoDBApplicationTestModule : AbpModule
{

}
