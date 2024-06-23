using Volo.Abp.Modularity;

namespace AbpF.Test.Angular.MongoDB;

/* Inherit from this class for your domain layer tests. */
public abstract class MongoDBDomainTestBase<TStartupModule> : MongoDBTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
