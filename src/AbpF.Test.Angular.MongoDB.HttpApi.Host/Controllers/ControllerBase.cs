using Volo.Abp.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;
using AbpF.Test.Angular.MongoDB.Members;

namespace AbpF.Test.Angular.MongoDB.Areas.Member.Controllers;

public class ControllerBase1 : AbpController
{
    public IConfiguration? Configuration { get; set; }

    protected UserInfoModel? CurrUser { get; private set; }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        if (context.HttpContext.Items["User"] is UserInfoModel user)
        {
            CurrUser = user;
        }
    }
}
