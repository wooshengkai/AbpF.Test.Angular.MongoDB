using AbpF.Test.Angular.MongoDB.Samples;
using Xunit;

namespace AbpF.Test.Angular.MongoDB.MongoDB.Domains;

[Collection(MongoDBTestConsts.CollectionDefinitionName)]
public class MongoDBSampleDomainTests : SampleDomainTests<MongoDBMongoDbTestModule>
{

}
