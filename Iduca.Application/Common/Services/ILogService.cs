using Iduca.Domain.Models;

namespace Iduca.Application.Common.Services;

public interface ILogService
{
    Task LogActionAsync(string message, string actionUrl, Guid userId, Guid? relatedId = null, bool status = true, CancellationToken cancellationToken = default);
    Task LogCreateAsync(string entityName, Guid entityId, Guid userId, CancellationToken cancellationToken = default);
    Task LogUpdateAsync(string entityName, Guid entityId, Guid userId, CancellationToken cancellationToken = default);
    Task LogDeleteAsync(string entityName, Guid entityId, Guid userId, CancellationToken cancellationToken = default);
    Task LogLoginAsync(Guid userId, bool success, CancellationToken cancellationToken = default);
}
