namespace Iduca.Application.Features.Categories.DeleteById;

public sealed record DeleteByIdCategoryResponse
(
    Guid Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? DisabledAt,
    string Name
);