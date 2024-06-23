using Volo.Abp.Settings;

namespace AbpF.Test.Angular.MongoDB.Settings;

public class MongoDBSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(MongoDBSettings.MySetting1));
    }
}
