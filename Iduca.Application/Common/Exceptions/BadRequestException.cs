using Iduca.Domain.Common.Enums;
using Iduca.Domain.Common.Messages;

namespace Iduca.Application.Common.Exceptions;

public class BadRequestException(
    string message = ExceptionMessage.BadRequest.Default,
    string? details = null
) : AppException(StatusCode.BadRequest, message, details) { }