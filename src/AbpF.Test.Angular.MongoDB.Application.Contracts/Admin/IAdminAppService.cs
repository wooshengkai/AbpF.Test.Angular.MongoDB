using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AbpF.Test.Angular.MongoDB.Admins;

public interface IAdminAppService : IApplicationService
{
    Task<bool> Add();

    Task<AdminDto> Get();
}