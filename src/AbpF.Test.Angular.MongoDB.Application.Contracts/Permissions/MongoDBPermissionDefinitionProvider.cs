using AbpF.Test.Angular.MongoDB.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace AbpF.Test.Angular.MongoDB.Permissions;

public class MongoDBPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(MongoDBPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(MongoDBPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<MongoDBResource>(name);
    }
}
