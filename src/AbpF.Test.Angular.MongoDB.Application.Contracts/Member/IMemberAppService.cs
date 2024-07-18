using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AbpF.Test.Angular.MongoDB.Members;

public interface IMemberAppService : IApplicationService
{
    Task<string> LoginAsync(MemberDto input);
}