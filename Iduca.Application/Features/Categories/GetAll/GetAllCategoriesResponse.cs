namespace Iduca.Application.Features.Categories.GetAll;

public sealed record GetAllCategoriesResponse(
    Guid Id,
    string Name,
    DateTime CreatedAt
);
