namespace Iduca.Application.Features.User.GetByQuery;

public sealed record GetUsersResponse(
    Guid Id,
    string Name,
    string Identity,
    string Email,
    bool IsAdmin,
    string? ResponsibleName,
    string CompanyName,
    string? Image,
    DateTime CreatedAt
);
