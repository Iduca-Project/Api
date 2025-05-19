using Iduca.Domain.Common.Enums;
using Iduca.Domain.Common.Messages;

namespace Iduca.Application.Common.Exceptions;

public class DuplicityException(
    string message = ExceptionMessage.DuplicityModel.Default,
    string? details = null
) : AppException(StatusCode.Conflict, message, details) { }