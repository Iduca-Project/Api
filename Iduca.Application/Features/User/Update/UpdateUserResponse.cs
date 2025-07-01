namespace Iduca.Application.Features.User.Update;

public sealed record UpdateUserResponse(
    Guid Id,
    string Name,
    string Identity,
    string Email,
    bool IsAdmin,
    Guid? ResponsibleId,
    Guid CompanyId,
    string? Image,
    DateTime UpdatedAt
);
