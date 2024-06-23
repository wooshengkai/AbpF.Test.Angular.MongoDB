using AbpF.Test.Angular.MongoDB.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace AbpF.Test.Angular.MongoDB.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class MongoDBController : AbpControllerBase
{
    protected MongoDBController()
    {
        LocalizationResource = typeof(MongoDBResource);
    }
}
