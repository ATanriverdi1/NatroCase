using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NatroCase.Api.Configuration;

namespace NatroCase.Api.Middlewares;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (ShouldBypassAuthorization(context))
        {
            await _next(context);
            return;
        }

        if (!IsTokenValid(context))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return;
        }
        
        await _next(context);
    }
    
    private bool ShouldBypassAuthorization(HttpContext context)
    {
        string path = context.Request.Path.HasValue ? context.Request.Path.Value : "";
        return path.StartsWith("/swagger") || path.StartsWith("/users/auth") || path.EndsWith("/users") || path.StartsWith("/domains");
    }

    private bool IsTokenValid(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var configuration = context.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;
        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>(ConfigKeys.SecretKey)));
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = secretKey,
            ValidateIssuer = false,
            ValidateAudience = false
        };
        
        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out _);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }
}