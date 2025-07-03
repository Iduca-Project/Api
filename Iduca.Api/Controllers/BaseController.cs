using Microsoft.AspNetCore.Mvc;

namespace Iduca.Api.Controllers;

/// <summary>
/// Controller base que fornece métodos auxiliares para todos os controllers
/// </summary>
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Obtém o ID do usuário logado a partir do contexto HTTP
    /// </summary>
    protected Guid GetCurrentUserId()
    {
        var userId = HttpContext.Items["UserId"];
        if (userId == null)
            throw new UnauthorizedAccessException("Usuário não autenticado");
        
        return (Guid)userId;
    }

    /// <summary>
    /// Verifica se o usuário logado é administrador
    /// </summary>
    protected bool IsCurrentUserAdmin()
    {
        var isAdmin = HttpContext.Items["IsAdmin"];
        return isAdmin != null && (bool)isAdmin;
    }

    /// <summary>
    /// Obtém o ID da empresa do usuário logado
    /// </summary>
    protected Guid GetCurrentUserCompanyId()
    {
        var companyId = HttpContext.Items["CompanyId"];
        if (companyId == null)
            throw new UnauthorizedAccessException("Empresa do usuário não encontrada");
        
        return (Guid)companyId;
    }

    /// <summary>
    /// Verifica se o usuário logado pode acessar dados de outro usuário
    /// baseado na hierarquia organizacional
    /// </summary>
    protected async Task<bool> CanAccessUserDataAsync(Guid targetUserId)
    {
        var currentUserId = GetCurrentUserId();
        
        // Admin pode acessar qualquer usuário
        if (IsCurrentUserAdmin())
            return true;
        
        // Usuário pode acessar seus próprios dados
        if (currentUserId == targetUserId)
            return true;
        
        // TODO: Implementar verificação de hierarquia real
        // var hierarchyService = HttpContext.RequestServices.GetRequiredService<IHierarchyService>();
        // return await hierarchyService.IsUserSuperiorToAsync(currentUserId, targetUserId);
        
        // Por enquanto, permite acesso (será implementado quando os handlers reais forem criados)
        return true;
    }
}
