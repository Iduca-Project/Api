using Iduca.Domain.Models;

namespace Iduca.Application.Features.Categories.GetByName;

public sealed record GetByNameCategoryResponse(
    List<Category> Categories
);