using Iduca.Application.Repository;
using Iduca.Application.Repository.UserRepository;
using Iduca.Domain.Models;

namespace Iduca.Application.Common.Services;

public interface IHierarchyService
{
    Task<bool> WouldCreateCycleAsync(Guid userId, Guid newResponsibleId, CancellationToken cancellationToken);
    Task<List<Guid>> GetAllSubordinatesAsync(Guid userId, bool includeIndirect = false, CancellationToken cancellationToken = default);
    Task<List<Guid>> GetSuperiorChainAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> IsUserSuperiorToAsync(Guid superiorId, Guid subordinateId, CancellationToken cancellationToken = default);
    Task<int> GetHierarchyLevelAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> AssignResponsibleAsync(Guid userId, Guid responsibleId, CancellationToken cancellationToken = default);
    Task<bool> RemoveResponsibleAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<List<User>> GetSubordinatesAsync(Guid userId, bool includeIndirect = false, CancellationToken cancellationToken = default);
    Task<List<User>> GetSuperiorsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<object> GetHierarchyTreeAsync(Guid companyId, Guid? rootUserId = null, CancellationToken cancellationToken = default);
    Task<bool> CanUserAccessDataAsync(Guid userId, Guid targetUserId, CancellationToken cancellationToken = default);
    Task<List<Guid>> GetAccessibleUserIdsAsync(Guid userId, CancellationToken cancellationToken = default);
}

public class HierarchyService : IHierarchyService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public HierarchyService(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> AssignResponsibleAsync(Guid userId, Guid responsibleId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.Get(userId, cancellationToken);
        var responsible = await _userRepository.Get(responsibleId, cancellationToken);

        if (user == null || responsible == null)
            return false;

        // Verificar se os usuários são da mesma empresa
        if (user.CompanyId != responsible.CompanyId)
            return false;

        // Verificar se não criaria um ciclo
        if (await WouldCreateCycleAsync(userId, responsibleId, cancellationToken))
            return false;

        // Atribuir responsável
        user.ResponsibleId = responsibleId;
        _userRepository.Update(user);
        await _unitOfWork.Save(cancellationToken);

        return true;
    }

    public async Task<bool> RemoveResponsibleAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.Get(userId, cancellationToken);

        if (user == null)
            return false;

        user.ResponsibleId = null;
        _userRepository.Update(user);
        await _unitOfWork.Save(cancellationToken);

        return true;
    }

    /// <summary>
    /// Verifica se atribuir um responsável criaria um ciclo na hierarquia
    /// </summary>
    public async Task<bool> WouldCreateCycleAsync(Guid userId, Guid newResponsibleId, CancellationToken cancellationToken)
    {
        // 1. Se o novo responsável é o próprio usuário, é um ciclo
        if (userId == newResponsibleId)
            return true;
        
        // 2. Se o novo responsável é subordinado (direto ou indireto) do usuário, criaria um ciclo
        var allSubordinates = await GetAllSubordinatesAsync(userId, includeIndirect: true, cancellationToken);
        if (allSubordinates.Contains(newResponsibleId))
            return true;
        
        // 3. Verificação adicional: percorrer a cadeia de superiores do novo responsável
        var superiorChain = await GetSuperiorChainAsync(newResponsibleId, cancellationToken);
        if (superiorChain.Contains(userId))
            return true;
        
        return false;
    }
    
    /// <summary>
    /// Obtém todos os subordinados de um usuário
    /// </summary>
    public async Task<List<Guid>> GetAllSubordinatesAsync(Guid userId, bool includeIndirect = false, CancellationToken cancellationToken = default)
    {
        var subordinateIds = new List<Guid>();
        
        // Buscar subordinados diretos
        var allUsers = await _userRepository.GetAll(cancellationToken);
        var directSubordinates = allUsers.Where(u => u.ResponsibleId == userId).ToList();
        
        subordinateIds.AddRange(directSubordinates.Select(u => u.Id));
        
        if (includeIndirect)
        {
            // Recursivamente buscar subordinados indiretos
            foreach (var directSubordinate in directSubordinates)
            {
                var indirectSubordinates = await GetAllSubordinatesAsync(directSubordinate.Id, true, cancellationToken);
                subordinateIds.AddRange(indirectSubordinates);
            }
        }
        
        return subordinateIds.Distinct().ToList();
    }
    
    /// <summary>
    /// Obtém a cadeia completa de superiores de um usuário
    /// </summary>
    public async Task<List<Guid>> GetSuperiorChainAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var superiorIds = new List<Guid>();
        var currentUser = await _userRepository.Get(userId, cancellationToken);
        
        while (currentUser?.ResponsibleId.HasValue == true)
        {
            superiorIds.Add(currentUser.ResponsibleId.Value);
            currentUser = await _userRepository.Get(currentUser.ResponsibleId.Value, cancellationToken);
        }
        
        return superiorIds;
    }
    
    /// <summary>
    /// Verifica se um usuário é superior a outro na hierarquia
    /// </summary>
    public async Task<bool> IsUserSuperiorToAsync(Guid superiorId, Guid subordinateId, CancellationToken cancellationToken = default)
    {
        if (superiorId == subordinateId)
            return false;

        var subordinateChain = await GetSuperiorChainAsync(subordinateId, cancellationToken);
        return subordinateChain.Contains(superiorId);
    }
    
    /// <summary>
    /// Obtém o nível hierárquico de um usuário (0 = topo, maior número = mais baixo)
    /// </summary>
    public async Task<int> GetHierarchyLevelAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var superiorChain = await GetSuperiorChainAsync(userId, cancellationToken);
        return superiorChain.Count;
    }

    public async Task<List<User>> GetSubordinatesAsync(Guid userId, bool includeIndirect = false, CancellationToken cancellationToken = default)
    {
        var subordinates = new List<User>();
        
        // Buscar subordinados diretos
        var allUsers = await _userRepository.GetAll(cancellationToken);
        var directSubordinates = allUsers.Where(u => u.ResponsibleId == userId).ToList();
        
        subordinates.AddRange(directSubordinates);
        
        if (includeIndirect)
        {
            // Recursivamente buscar subordinados indiretos
            foreach (var directSubordinate in directSubordinates)
            {
                var indirectSubordinates = await GetSubordinatesAsync(directSubordinate.Id, true, cancellationToken);
                subordinates.AddRange(indirectSubordinates);
            }
        }
        
        return subordinates.Distinct().ToList();
    }

    public async Task<List<User>> GetSuperiorsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var superiors = new List<User>();
        var currentUser = await _userRepository.Get(userId, cancellationToken);

        while (currentUser?.ResponsibleId.HasValue == true)
        {
            var superior = await _userRepository.Get(currentUser.ResponsibleId.Value, cancellationToken);
            if (superior != null)
            {
                superiors.Add(superior);
                currentUser = superior;
            }
            else
            {
                break;
            }
        }

        return superiors;
    }

    public async Task<object> GetHierarchyTreeAsync(Guid companyId, Guid? rootUserId = null, CancellationToken cancellationToken = default)
    {
        // Buscar todos os usuários da empresa
        var users = await _userRepository.GetUsersByCompany(companyId, cancellationToken);

        if (!users.Any())
            return new { nodes = new List<object>() };

        // Se rootUserId não foi especificado, buscar usuários sem responsável (raízes)
        if (!rootUserId.HasValue)
        {
            var rootUsers = users.Where(u => u.ResponsibleId == null).ToList();
            var trees = new List<object>();

            foreach (var rootUser in rootUsers)
            {
                trees.Add(await BuildUserTreeAsync(rootUser, users, cancellationToken));
            }

            return new { nodes = trees };
        }

        // Construir árvore a partir do usuário especificado
        var specifiedUser = users.FirstOrDefault(u => u.Id == rootUserId.Value);
        if (specifiedUser == null)
            return new { nodes = new List<object>() };

        var tree = await BuildUserTreeAsync(specifiedUser, users, cancellationToken);
        return new { nodes = new List<object> { tree } };
    }

    private async Task<object> BuildUserTreeAsync(User user, List<User> allUsers, CancellationToken cancellationToken = default)
    {
        var subordinates = allUsers.Where(u => u.ResponsibleId == user.Id).ToList();
        var children = new List<object>();

        foreach (var subordinate in subordinates)
        {
            children.Add(await BuildUserTreeAsync(subordinate, allUsers, cancellationToken));
        }

        return new
        {
            id = user.Id,
            name = user.Name,
            email = user.Email,
            isAdmin = user.IsAdmin,
            responsibleId = user.ResponsibleId,
            children = children
        };
    }

    public async Task<bool> CanUserAccessDataAsync(Guid userId, Guid targetUserId, CancellationToken cancellationToken = default)
    {
        // Um usuário pode acessar seus próprios dados
        if (userId == targetUserId)
            return true;

        // Verificar se o usuário é admin
        var user = await _userRepository.Get(userId, cancellationToken);
        if (user?.IsAdmin == true)
            return true;

        // Verificar se o usuário é superior ao usuário alvo
        return await IsUserSuperiorToAsync(userId, targetUserId, cancellationToken);
    }

    public async Task<List<Guid>> GetAccessibleUserIdsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.Get(userId, cancellationToken);
        if (user == null)
            return new List<Guid>();

        var accessibleIds = new List<Guid> { userId }; // Sempre pode acessar seus próprios dados

        if (user.IsAdmin)
        {
            // Admins podem acessar todos os usuários da empresa
            var companyUsers = await _userRepository.GetUsersByCompany(user.CompanyId, cancellationToken);
            accessibleIds.AddRange(companyUsers.Select(u => u.Id));
        }
        else
        {
            // Usuários normais podem acessar apenas seus subordinados
            var subordinateIds = await GetAllSubordinatesAsync(userId, true, cancellationToken);
            accessibleIds.AddRange(subordinateIds);
        }

        return accessibleIds.Distinct().ToList();
    }
}
