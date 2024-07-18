using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Swashbuckle;
using Volo.Abp.VirtualFileSystem;
using AbpF.Test.Angular.MongoDB.MongoDB;
using Autofac.Core;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Volo.Abp.AspNetCore.Uow;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.IdentityModel.Tokens;
using System.Text;
using AbpF.Test.Angular.MongoDB.Members;


namespace AbpF.Test.Angular.MongoDB;

[DependsOn(
    //typeof(MongoDBHttpApiModule), //* 因为没有使用 ABP Framework 项目自带的功能，所以不使用
    //typeof(MongoDBApplicationModule), //* 因因为没有使用 ABP Framework 项目自带的功能，所以不使用
    typeof(MongoDBMongoDbModule),
    typeof(AbpAutofacModule),
    //typeof(AbpAccountWebOpenIddictModule), //* 因为对 OpenIddict 还不熟悉所以就暂不使用了
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
)]
public class MongoDBHttpApiCopyModule : AbpModule
{
    /// <summary>
    /// PreConfigureServices 是 ABP Framework 中的一种约定方法，用于在服务配置阶段执行预配置操作。这些操作在实际服务注册之前执行，允许开发者在服务注册之前进行额外的配置
    /// </summary>
    /// <param name="context">是 launchSettings.json 里的配置信息</param>
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        // OpenIddictBuilder 是 OpenIddict 库提供的构建器，用于配置和添加 OpenID Connect 和 OAuth 2.0 认证服务。
        //PreConfigure<OpenIddictBuilder>(builder =>
        //{
        //    /// OpenIddictBuilder 是 OpenIddict 库提供的构建器，用于配置和添加 OpenID Connect 和 OAuth 2.0 认证服务。
        //    builder.AddValidation(options =>
        //    {
        //        /// AddAudiences 方法用于添加受众（audience）信息到验证配置中。在这里，"MongoDB" 是指定的受众名称。
        //        options.AddAudiences("MongoDB");
        //        /// UseLocalServer 方法配置 OpenIddict 使用本地服务器模式。本地服务器模式是 OpenID Connect 和 OAuth 2.0 的一种模式，用于在同一应用程序内处理令牌和验证请求。
        //        //options.UseLocalServer();
        //        /// UseAspNetCore 方法配置 OpenIddict 使用 ASP.NET Core 的集成。这包括集成到 ASP.NET Core 的身份验证和授权管道中，以便 OpenIddict 可以与应用程序的认证机制无缝集成。
        //        //options.UseAspNetCore();
        //    });
        //});
    }

    /// <summary>
    /// 进行服务配置
    /// </summary>
    /// <param name="context">是 launchSettings.json 里的配置信息</param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        /// context.Services.GetConfiguration() 获取应用程序的配置对象，通常是从 appsettings.json 等配置文件中读取的配置信息。
        var configuration = context.Services.GetConfiguration();
        /// context.Services.GetHostingEnvironment() 获取当前的宿主环境对象，可以用来检查当前的运行环境（如开发环境、生产环境等）。
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        /// 配置身份验证服务的方法，具体实现未显示，通常用于设置应用程序的认证机制。
        //ConfigureAuthentication(context);
        ConfigureAuthenticationTest1(context, configuration);
        /// 配置虚拟文件系统的方法，具体实现未显示，通常用于设置 ABP Framework 的虚拟文件系统，以便在应用程序中使用嵌入资源文件。
        ConfigureVirtualFileSystem(context);
        /// 配置跨域资源共享 (CORS) 的方法，具体实现未显示，通常用于设置哪些域名可以访问该应用程序的资源。
        ConfigureCors(context, configuration);

        //* 在开发模式时方可注册  SwaggerServices
        if (hostingEnvironment.IsDevelopment())
        {
            /// 配置 Swagger 服务的方法，Swagger 是一个用于生成和展示 API 文档的工具，通常在开发阶段启用，以便开发者可以方便地测试和查看 API 文档。
            ConfigureSwaggerServices(context, configuration);
        }
    }

    /// <summary>
    /// ConfigureAuthentication 是在 ABP Framework 应用程序中配置身份验证服务
    /// </summary>
    /// <param name="context">是 launchSettings.json 里的配置信息</param>
    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        /// ForwardIdentityAuthenticationForBearer 是一个扩展方法，用于将 Bearer 令牌的身份验证转发到指定的身份验证方案。
        /// 这段代码的作用是将所有 Bearer 令牌的身份验证请求转发到 OpenIddict 的验证方案。这意味着应用程序会使用 OpenIddict 验证传入的 Bearer 令牌，从而实现基于 OAuth 2.0 或 OpenID Connect 的身份验证。
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        /// 启用动态声明功能意味着应用程序在创建 ClaimsPrincipal 对象时，可以动态地添加或修改声明。这对于根据当前用户的状态或上下文动态生成额外的声明信息非常有用。
        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });
    }

    private void ConfigureAuthenticationTest1(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });
        context.Services.AddControllers();
        context.Services.AddTransient<IMemberAppService, MemberAppService>();
    }

    /// <summary>
    /// ConfigureVirtualFileSystem 是配置虚拟文件系统（Virtual File System，VFS）以支持开发环境下的物理文件替换。
    /// 在 ABP Framework 中，虚拟文件系统用于处理嵌入式资源文件，如嵌入的视图、静态文件等。这段代码实现了在开发环境中用物理文件替换嵌入式资源文件。
    /// 作用总结：这段代码在开发环境中通过物理文件替换嵌入的资源文件，以便开发者可以直接修改物理文件而无需重新编译嵌入资源。这种配置方式极大地提高了开发效率，使开发者能够即时查看修改效果，而无需进行额外的编译步骤。
    /// </summary>
    /// <param name="context">是 launchSettings.json 里的配置信息</param>
    private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        /// 在开发模式时方可将嵌入式文件集替换为物理文件集
        if (hostingEnvironment.IsDevelopment())
        {
            /// 配置 AbpVirtualFileSystemOptions 选项。
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                /// options.FileSets.ReplaceEmbeddedByPhysical<TModule> 方法用于将嵌入式文件集替换为物理文件集。
                /// Path.Combine() 构建物理文件路径。通过使用 .. 和 Path.DirectorySeparatorChar，定位到项目的物理文件路径。

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

    /// <summary>
    /// ConfigureSwaggerServices 是用于在 ABP Framework 应用程序中配置 Swagger 服务，包括 OAuth 认证集成和 API 文档生成。
    /// Swagger 是一个用于生成和展示 API 文档的工具，通常在开发阶段启用，以便开发者可以方便地测试和查看 API 文档。
    /// </summary>
    /// <param name="context">是 launchSettings.json 里的配置信息</param>
    /// <param name="configuration">是 appsettings.json 里的配置信息</param>
    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAbpSwaggerGenWithOAuth(
            configuration["AuthServer:Authority"]!,
            new Dictionary<string, string>
            {
                {"MongoDB", "MongoDB API"}
            },
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "MongoDB API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) =>
                //true);
                {
                    // 获取控制器类型信息
                    var controllerActionDescriptor = description.ActionDescriptor as ControllerActionDescriptor;
                    if (controllerActionDescriptor != null)
                    {
                        // 检查是否存在区域（Areas）属性
                        var areaAttribute = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(AreaAttribute), true).FirstOrDefault();
                        if (areaAttribute != null)
                        {
                            return true;
                        }
                    }

                    return true; // 其他路由
                });

                // 另一种写法
                //options.DocInclusionPredicate((_, description) => description.RelativePath != null && description.RelativePath switch
                //{
                //    _ when description.RelativePath.StartsWith("api/app") => false,
                //    _ when description.RelativePath.StartsWith("api/abp") => false,
                //    _ => true
                //});

                options.CustomSchemaIds(type => type.FullName);
            });
    }

    /// <summary>
    /// 配置跨域资源共享 (CORS) 的方法，具体实现未显示，通常用于设置哪些域名可以访问该应用程序的资源。
    /// 在 ABP Framework 应用程序中配置跨域资源共享 (CORS) 服务。具体地，配置了一个默认的 CORS 策略，用于控制哪些源可以访问该应用程序的资源。
    /// </summary>
    /// <param name="context">是 launchSettings.json 里的配置信息</param>
    /// <param name="configuration">是 appsettings.json 里的配置信息</param>
    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        /// context.Services.AddCors 方法用于注册 CORS 服务。
        context.Services.AddCors(options =>
        {
            /// options.AddDefaultPolicy 方法用于配置一个默认的 CORS 策略。
            options.AddDefaultPolicy(builder =>
            {
                /// builder.WithOrigins 方法配置允许的源
                builder
                    /// configuration["App:CorsOrigins"] 从配置中读取允许的源（多个源用逗号分隔）。
                    .WithOrigins(configuration["App:CorsOrigins"]?
                        /// Split(",", StringSplitOptions.RemoveEmptyEntries) 将这些源拆分为数组，并移除空条目。
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        /// Select(o => o.RemovePostFix("/")) 移除每个源 URL 的尾部斜杠（假如有的话）。
                        .Select(o => o.RemovePostFix("/"))
                        .ToArray() ?? Array.Empty<string>())
                    /// WithAbpExposedHeaders 是 ABP Framework 提供的扩展方法，用于配置需要在 CORS 响应中暴露的头部。
                    .WithAbpExposedHeaders()
                    /// SetIsOriginAllowedToAllowWildcardSubdomains 允许使用通配符子域。
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    // error=> 这个还不知道需不需要加，chatgpt 是说是要加
                    //.AllowAnyOrigin()
                    // error=>
                    /// AllowAnyHeader 允许任何请求头。
                    .AllowAnyHeader()
                    /// AllowAnyMethod 允许任何 HTTP 方法（如 GET、POST、PUT、DELETE 等）。
                    .AllowAnyMethod()
                    /// AllowCredentials 允许跨域请求发送凭据（如 Cookie、HTTP 认证等）。
                    .AllowCredentials();
            });
        });
    }

    /// <summary>
    /// OnApplicationInitialization 方法，用于配置 ASP.NET Core 应用程序的中间件管道和初始化设置。
    /// 这个方法在应用程序初始化时调用，配置了开发和生产环境中的不同行为，确保应用程序在不同的环境下能正常运行。
    /// </summary>
    /// <param name="context"></param>
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        var configuration1 = context.ServiceProvider.GetRequiredService<IConfiguration>();
        var asd = configuration1["Test:Id"];

        if (env.IsDevelopment())
        {
            /// app.UseDeveloperExceptionPage() 启用开发者异常页面，显示详细的错误信息，方便调试。
            app.UseDeveloperExceptionPage();

            // 在开发模式时方可开启使用 Swagger 文档功能
            app.UseSwagger();
            app.UseAbpSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MongoDB API");

                var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
                c.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                c.OAuthScopes("MongoDB");
                //* 可以关闭在 Swagger 中的 Schemas
                c.DefaultModelsExpandDepth(-1);
            });
        }

        /// 配置本地化中间件，支持多语言和文化设置。
        // 暂时保留多语言切换设置
        //app.UseAbpRequestLocalization();
        //error=> 这个还不知道需不需要加，chatgpt 是说是要加
        if (!env.IsDevelopment())
        {
            //// 这个是有需要使用到这个 using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
            //app.UseErrorPage();
        }
        // error=>
        /// app.UseCorrelationId()：添加一个关联 ID（Correlation ID）中间件，用于跟踪请求链中的所有请求。
        app.UseCorrelationId();
        /// app.UseStaticFiles()：启用静态文件中间件，服务于 wwwroot 文件夹中的静态文件，如 HTML、CSS、JavaScript、图像等。
        app.UseStaticFiles();
        /// app.UseRouting()：启用路由中间件，处理路由匹配。
        app.UseRouting();
        /// app.UseCors()：启用 CORS 中间件，允许跨域请求。
        app.UseCors();
        // 使用身份验证中间件
        app.UseMiddleware<AuthenticationMiddleware>();
        /// app.UseAuthentication()：启用身份验证中间件。
        //app.UseAuthentication();
        /// app.UseAbpOpenIddictValidation()：启用 ABP 的 OpenIddict 验证中间件。
        //app.UseAbpOpenIddictValidation();
        app.UseMiddleware<AbpUnitOfWorkMiddleware>();
        /// app.UseUnitOfWork()：启用工作单元（Unit of Work）中间件，管理数据库事务。
        app.UseUnitOfWork();
        /// app.UseDynamicClaims()：启用动态声明中间件，处理用户声明。
        //app.UseDynamicClaims();
        /// app.UseAuthorization()：启用授权中间件。
        //app.UseAuthorization();
        /// app.UseAuditing()：启用审计中间件，记录请求日志
        app.UseAuditing();
        /// app.UseAbpSerilogEnrichers()：启用 Serilog 日志增强中间件。
        app.UseAbpSerilogEnrichers();
        /// app.UseConfiguredEndpoints()：启用已配置的端点处理。
        app.UseConfiguredEndpoints();
    }
}
