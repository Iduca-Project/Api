namespace Iduca.Application.Features.Categories.Create;

public sealed record CreateCategoryResponse(
    Guid Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? DisabledAt,
    string Name
);