using Iduca.Application.Common.Exceptions;
using Iduca.Application.Common.Session;
using Iduca.Domain.Common.Messages;
using Iduca.Domain.Objects;

namespace IAgro.API.Security;

public class RequestSession : IRequestSession
{
    private SessionData? SessionData { get; set; }

    public SessionData GetSessionOrThrow()
        => SessionData ?? throw new UnauthorizedException(
            ExceptionMessage.Unauthorized.Session);

    public void SetSession(SessionData? sessionData)
        => SessionData = sessionData;
}