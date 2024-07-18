//using Volo.Abp.Account;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Caching;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
//using Volo.Abp.TenantManagement;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Caching;
using AbpF.Test.Angular.MongoDB.MyRedis;

namespace AbpF.Test.Angular.MongoDB;

[DependsOn(
    typeof(MongoDBDomainModule),
    ///typeof(AbpAccountApplicationModule),
    typeof(MongoDBApplicationContractsModule),
    //typeof(AbpIdentityApplicationModule),
    //typeof(AbpPermissionManagementApplicationModule),
    //typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule)
    //, typeof(AbpCachingModule)
    //, typeof(AbpCachingStackExchangeRedisModule)
    , typeof(MyCachingModule)
    )]
public class MongoDBApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //var configuration = context.Services.GetConfiguration();

        //Configure<AbpRedisCacheOptions>(options =>
        //{
        //    options.Configuration = configuration["Redis:Configuration"];
        //});

        //Configure<AbpDistributedCacheOptions>(options =>
        //{
        //    options.KeyPrefix = "MyApp:";
        //});

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<MongoDBApplicationModule>();
        });
    }
}
