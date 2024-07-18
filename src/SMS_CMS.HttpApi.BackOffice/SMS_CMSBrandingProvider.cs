using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace SMS_CMS;

[Dependency(ReplaceServices = true)]
public class SMS_CMSBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "SMS_CMS";
}
