namespace Iduca.Application.Features.Categories.Get;

public sealed record GetCategoryResponse(
    Guid Id,
    string Name,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
