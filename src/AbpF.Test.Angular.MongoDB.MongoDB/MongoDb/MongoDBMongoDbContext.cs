using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using AbpF.Test.Angular.MongoDB.Admins;
using Volo.Abp.PermissionManagement.MongoDB;
using Volo.Abp.PermissionManagement;
using Volo.Abp;

namespace AbpF.Test.Angular.MongoDB.MongoDB;

[ConnectionStringName("Default")]
public class MongoDBMongoDbContext : AbpMongoDbContext
{
    /* Add mongo collections here. Example:
     * public IMongoCollection<Question> Questions => Collection<Question>();
     */
    public IMongoCollection<AdminDB> Admin => Collection<AdminDB>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        //modelBuilder.Entity<YourEntity>(b =>
        //{
        //    //...
        //});

        //modelBuilder.ConfigurePermissionManagement();

        Check.NotNull(modelBuilder, nameof(modelBuilder));

        modelBuilder.Entity<AdminDB>(b =>
        {
            b.CollectionName = "AdminDB";
        });
    }
}
