using Iduca.Domain.Objects;

namespace Iduca.Application.Common.Session;

public interface IRequestSession
{
    SessionData GetSessionOrThrow();
    void SetSession(SessionData? sessionData);
}