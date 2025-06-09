using Iduca.Domain.Common.Enums;
using Iduca.Domain.Common.Messages;

namespace Iduca.Application.Common.Exceptions;

public class UnauthorizedException(
    string message = ExceptionMessage.Unauthorized.Default,
    string? details = null
) : AppException(StatusCode.Unauthorized, message, details) { }