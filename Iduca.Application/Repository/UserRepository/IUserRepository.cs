using Iduca.Domain.Models;

namespace Iduca.Application.Repository.UserRepository;

public interface IUserRepository : IBaseRepository<User> 
{
    Task<User?> GetUserByEmail(string email, CancellationToken cancellationToken);
    Task<User?> GetUserByIdentity(string identity, CancellationToken cancellationToken);
    Task<bool> IsManager(Guid id, CancellationToken cancellationToken);
    Task<List<User>> GetUsersByCompany(Guid companyId, CancellationToken cancellationToken);
    Task<User?> GetUserWithDetails(Guid id, CancellationToken cancellationToken);
    Task<List<User>> GetUsersByQuery(string? name, string? email, Guid? companyId, bool? isAdmin, int page, int maxItems, CancellationToken cancellationToken);
    
    // Métodos para autenticação
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<bool> IsFirstAccessAsync(Guid userId, CancellationToken cancellationToken);
    Task MarkFirstAccessCompleteAsync(Guid userId, CancellationToken cancellationToken);
}