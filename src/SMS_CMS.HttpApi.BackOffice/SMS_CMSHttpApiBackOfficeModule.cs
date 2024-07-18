using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.VirtualFileSystem;
using AbpF.Test.Angular.MongoDB.MongoDB;
using AbpF.Test.Angular.MongoDB;

namespace SMS_CMS;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(MongoDBHttpApiModule),
    typeof(MongoDBApplicationModule),
    typeof(MongoDBMongoDbModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
)]
public class SMS_CMSHttpApiBackOfficeModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        IConfiguration configuration = context.Services.GetConfiguration();
        IWebHostEnvironment hostingEnvironment = context.Services.GetHostingEnvironment();

        ConfigureAuthentication(context);
        ConfigureVirtualFileSystem(context);
        ConfigureCors(context, configuration);

        // 只在开发模式时方可开启使用 Swagger 文档功能
        if (hostingEnvironment.IsDevelopment())
        {
            ConfigureSwaggerServices(context, configuration);
        }
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
    }

    private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
    {
        IWebHostEnvironment hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<MongoDBDomainSharedModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}AbpF.Test.Angular.MongoDB.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<MongoDBDomainModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}AbpF.Test.Angular.MongoDB.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<MongoDBApplicationContractsModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}AbpF.Test.Angular.MongoDB.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<MongoDBApplicationModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}AbpF.Test.Angular.MongoDB.Application"));
            });
        }
    }

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAbpSwaggerGenWithOAuth(
            configuration["AuthServer:Authority"]!,
            new Dictionary<string, string>
            {
                {"SMS_CMS", "SMS_CMS API"}
            },
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "SMS_CMS API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(configuration["App:CorsOrigins"]?
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.RemovePostFix("/"))
                        .ToArray() ?? Array.Empty<string>())
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        IApplicationBuilder app = context.GetApplicationBuilder();
        IWebHostEnvironment env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            // 只在开发模式时方可开启使用 Swagger 文档功能
            app.UseSwagger();
            app.UseAbpSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SMS_CMS API");

                var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
                c.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                c.OAuthScopes("SMS_CMS");
                // 关闭在 Swagger 中的 Schemas
                c.DefaultModelsExpandDepth(-1);
            });
        }

        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();

        // 使用身份验证中间件（这个是自己编写的）
        //app.UseMiddleware<AuthenticationMiddleware>();

        // 使用中间件约束返回结果（这个是自己编写的）
        //app.UseMiddleware<AuthenticationMiddleware>();

        app.UseUnitOfWork();
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
