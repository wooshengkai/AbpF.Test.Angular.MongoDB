using AbpF.Test.Angular.MongoDB.MongoDB;
using AbpF.Test.Angular.MongoDB.Samples;
using Xunit;

namespace AbpF.Test.Angular.MongoDB.MongoDb.Applications;

[Collection(MongoDBTestConsts.CollectionDefinitionName)]
public class MongoDBSampleAppServiceTests : SampleAppServiceTests<MongoDBMongoDbTestModule>
{

}
