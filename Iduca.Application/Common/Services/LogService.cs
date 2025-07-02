using Iduca.Application.Repository;
using Iduca.Application.Repository.UserRepository;
using Iduca.Domain.Models;
using Iduca.Application.Repository.LogRepository;

namespace Iduca.Application.Common.Services;

public class LogService : ILogService
{
    private readonly ILogRepository _logRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogService(ILogRepository logRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _logRepository = logRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task LogActionAsync(string message, string actionUrl, Guid userId, Guid? relatedId = null, bool status = true, CancellationToken cancellationToken = default)
    {
        // Buscar o usuário para satisfazer a propriedade required
        var user = await _userRepository.Get(userId, cancellationToken);
        
        // Se o usuário não existe, não criar o log
        if (user == null) return;

        var log = new Log
        {
            Message = message,
            ActionUrl = actionUrl,
            UserId = userId,
            User = user,
            RelatedId = relatedId,
            Status = status,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _logRepository.Create(log);
        await _unitOfWork.Save(cancellationToken);
    }

    public async Task LogCreateAsync(string entityName, Guid entityId, Guid userId, CancellationToken cancellationToken = default)
    {
        await LogActionAsync(
            $"Criado {entityName} com ID {entityId}",
            $"/api/{entityName.ToLower()}",
            userId,
            entityId,
            true,
            cancellationToken
        );
    }

    public async Task LogUpdateAsync(string entityName, Guid entityId, Guid userId, CancellationToken cancellationToken = default)
    {
        await LogActionAsync(
            $"Atualizado {entityName} com ID {entityId}",
            $"/api/{entityName.ToLower()}",
            userId,
            entityId,
            true,
            cancellationToken
        );
    }

    public async Task LogDeleteAsync(string entityName, Guid entityId, Guid userId, CancellationToken cancellationToken = default)
    {
        await LogActionAsync(
            $"Deletado {entityName} com ID {entityId}",
            $"/api/{entityName.ToLower()}",
            userId,
            entityId,
            true,
            cancellationToken
        );
    }

    public async Task LogLoginAsync(Guid userId, bool success, CancellationToken cancellationToken = default)
    {
        // Para falhas de login onde o usuário não foi encontrado, não criar log
        if (userId == Guid.Empty) return;
        
        await LogActionAsync(
            success ? "Login realizado com sucesso" : "Tentativa de login falhada",
            "/api/auth/login",
            userId,
            userId,
            success,
            cancellationToken
        );
    }
}
