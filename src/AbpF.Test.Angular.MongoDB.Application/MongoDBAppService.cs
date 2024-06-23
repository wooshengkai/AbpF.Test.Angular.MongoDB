using System;
using System.Collections.Generic;
using System.Text;
using AbpF.Test.Angular.MongoDB.Localization;
using Volo.Abp.Application.Services;

namespace AbpF.Test.Angular.MongoDB;

/* Inherit your application services from this class.
 */
public abstract class MongoDBAppService : ApplicationService
{
    protected MongoDBAppService()
    {
        LocalizationResource = typeof(MongoDBResource);
    }
}
