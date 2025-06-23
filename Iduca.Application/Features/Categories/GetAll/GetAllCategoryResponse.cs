using Iduca.Domain.Models;

namespace Iduca.Application.Features.Categories.GetAll;

public sealed record GetAllCategoryResponse(
    List<CategoryDto> Categories
);

public sealed record CategoryDto(
    string Name,
    Guid Id
);
