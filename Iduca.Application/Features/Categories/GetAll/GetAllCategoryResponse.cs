using Iduca.Domain.Models;

namespace Iduca.Application.Features.Categories.GetAll;

public sealed record GetAllCategoryResponse(
    List<Category> Categories
);
