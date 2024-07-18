using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace AbpF.Test.Angular.MongoDB;

[Dependency(ReplaceServices = true)]
public class MongoDBBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "MongoDB";
}
