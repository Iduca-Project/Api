using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Iduca.Application.Contracts;
using System.Text.Json;
using Iduca.Application.Repository.UserRepository;

namespace Iduca.Api.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public bool RequireAdmin { get; set; } = false;
    public bool RequireAManager { get; set; } = false;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Verificar se há header Authorization
        var authHeader = context.HttpContext.Request.Headers.Authorization.FirstOrDefault();

        if (string.IsNullOrEmpty(authHeader))
        {
            context.Result = new UnauthorizedObjectResult(new { message = "Token de acesso requerido" });
            return;
        }

        if (!authHeader.StartsWith("Bearer "))
        {
            context.Result = new UnauthorizedObjectResult(new { message = "Formato de token inválido. Use: Bearer <token>" });
            return;
        }

        var token = authHeader["Bearer ".Length..];

        try
        {
            var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthenticator>();
            var sessionData = authService.ExtractToken(token);



            // Verificar se é admin quando necessário
            if (RequireAdmin && !sessionData.IsAdmin)
            {
                context.Result = new ForbidResult();
                return;
            }

            // Adicionar dados do usuário ao contexto para uso posterior
            context.HttpContext.Items["UserId"] = sessionData.UserId;
            context.HttpContext.Items["IsAdmin"] = sessionData.IsAdmin;
            context.HttpContext.Items["CompanyId"] = sessionData.CompanyId;
            context.HttpContext.Items["SessionData"] = sessionData;
        }
        catch (Exception ex)
        {
            context.Result = new UnauthorizedObjectResult(new { message = "Token inválido ou expirado", details = ex.Message });
        }
    }
}
