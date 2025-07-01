namespace Iduca.Application.Features.User.Get;

public sealed record GetUserResponse(
    Guid Id,
    string Name,
    string Identity,
    string Email,
    bool IsAdmin,
    Guid? ResponsibleId,
    string? ResponsibleName,
    Guid CompanyId,
    string CompanyName,
    string? Image,
    List<CategoryInfo> Interests,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public sealed record CategoryInfo(
    Guid Id,
    string Name
);
