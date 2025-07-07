using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Iduca.Application.Common.Services;

namespace Iduca.Api.Attributes;

/// <summary>
/// Atributo para autorização baseada em hierarquia organizacional
/// Verifica se o usuário logado tem permissão para acessar dados de outros usuários
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ManagerAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public bool RequireDirectSubordinate { get; set; } = true;
    public bool AllowSelf { get; set; } = true;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        try
        {
            // Obter dados da sessão (já devem estar disponíveis do CustomAuthorizeAttribute)
            var sessionData = context.HttpContext.Items["SessionData"];
            if (sessionData == null)
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Sessão não encontrada" });
                return;
            }

            // Se for Admin, pode acessar tudo
            var isAdmin = (bool)(context.HttpContext.Items["IsAdmin"] ?? false);
            if (isAdmin)
                return;

            // TODO: Implementar verificação de hierarquia real
            // Por enquanto, permite acesso (será implementado nos handlers)
            
            // Obter parâmetros da rota para verificar se há targetUserId
            var routeValues = context.RouteData.Values;
            var queryParams = context.HttpContext.Request.Query;
            
            // Exemplos de parâmetros que podem conter usuário alvo:
            // - {id} na rota
            // - {userId} na rota  
            // - employeeId no query
            // - userId no body (para POST/PUT)
            
            // var currentUserId = (Guid)context.HttpContext.Items["UserId"];
            // var hierarchyService = context.HttpContext.RequestServices.GetRequiredService<IHierarchyService>();
            
            // Se chegou até aqui sem problemas, permite acesso
            // A verificação detalhada será feita nos handlers específicos
        }
        catch (Exception ex)
        {
            context.Result = new UnauthorizedObjectResult(new { message = "Erro na verificação de hierarquia", details = ex.Message });
        }
    }
}
