using Volo.Abp.Modularity;

namespace AbpF.Test.Angular.MongoDB;

public abstract class MongoDBApplicationTestBase<TStartupModule> : MongoDBTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
