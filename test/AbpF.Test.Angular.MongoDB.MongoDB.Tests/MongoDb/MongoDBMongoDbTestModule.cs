using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace AbpF.Test.Angular.MongoDB.MongoDB;

[DependsOn(
    typeof(MongoDBApplicationTestModule),
    typeof(MongoDBMongoDbModule)
)]
public class MongoDBMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDBMongoDbFixture.GetRandomConnectionString();
        });
    }
}
