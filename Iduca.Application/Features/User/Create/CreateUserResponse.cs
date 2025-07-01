namespace Iduca.Application.Features.User.Create;

public sealed record CreateUserResponse(
    Guid Id,
    string Name,
    string Identity,
    string Email,
    bool IsAdmin,
    Guid? ResponsibleId,
    Guid CompanyId,
    string? Image,
    DateTime CreatedAt
);
