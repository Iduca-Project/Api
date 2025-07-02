namespace Iduca.Application.Common.Services;

public interface ISeedService
{
    Task EnsureDefaultDataAsync(CancellationToken cancellationToken = default);
}
