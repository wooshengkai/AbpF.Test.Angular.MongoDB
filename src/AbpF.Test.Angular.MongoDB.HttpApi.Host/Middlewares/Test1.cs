using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Text.Json;

public class TestMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _env;

    public TestMiddleware(RequestDelegate next, IConfiguration configuration, IWebHostEnvironment env)
    {
        _next = next;
        _configuration = configuration;
        _env = env;
    }

    public async Task Invoke1(HttpContext context)
    {
        // 在这里处理身份验证逻辑
        var token = context.Request.Headers["Authorization"].ToString();

        if (!string.IsNullOrEmpty(token))
        {
            // 验证token并创建身份
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "User Name"),
                new Claim(ClaimTypes.Role, "User Role")
            };

            var identity = new ClaimsIdentity(claims, "Custom");
            var principal = new ClaimsPrincipal(identity);
            context.User = principal;
        }
        else
        {
            //context.Response.StatusCode = 401;
            //await context.Response.WriteAsync("Unauthorized");
            //return;
        }

        await _next(context);
    }

    public async Task Invoke2(HttpContext context)
    {
        var asd = context.Request.Path;

        if (!context.Request.Path.StartsWithSegments("/member/Member/login") && (_env.IsDevelopment() && context.Request.Path != "/"))
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token == null || !ValidateToken(token))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }
        }

        await _next(context);
    }

    public async Task Invoke(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using (var responseBodyStream = new MemoryStream())
        {
            context.Response.Body = responseBodyStream;

            await _next(context);

            context.Response.Body = originalBodyStream;

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(responseBodyStream).ReadToEndAsync();
            responseBodyStream.Seek(0, SeekOrigin.Begin);

            var code = context.Items["code"] as int? ?? context.Response.StatusCode;
            var message = context.Items["message"] as string ?? GetDefaultMessageForStatusCode(code);
            object data = null;

            if (!string.IsNullOrEmpty(responseBodyText))
            {
                try
                {
                    data = JsonSerializer.Deserialize<object>(responseBodyText);
                }
                catch (JsonException)
                {
                    data = responseBodyText;
                }
            }

            var response = new ApiResponse<object>(code, message, data);

            var responseJson = JsonSerializer.Serialize(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code;
            await context.Response.WriteAsync(responseJson);
        }
    }

    private string GetDefaultMessageForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            200 => "OK",
            201 => "Created",
            400 => "Bad Request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Not Found",
            500 => "Internal Server Error",
            _ => "Error"
        };
    }

    private bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            }, out SecurityToken validatedToken);
        }
        catch
        {
            return false;
        }

        return true;
    }

    private class ApiResponse<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ApiResponse(int code, string message, T data)
        {
            Code = code;
            Message = message;
            Data = data;
        }
    }
}