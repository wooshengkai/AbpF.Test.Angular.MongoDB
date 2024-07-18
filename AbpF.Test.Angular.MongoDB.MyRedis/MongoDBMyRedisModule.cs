using Volo.Abp.Modularity;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;

namespace AbpF.Test.Angular.MongoDB.MyRedis
{
    [DependsOn(
        typeof(AbpCachingStackExchangeRedisModule)
    )]
    public class MyCachingModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //Configure<AbpDistributedCacheOptions>(options =>
            //{
            //    options.KeyPrefix = "MyApp:";
            //});
        }
    }
}
