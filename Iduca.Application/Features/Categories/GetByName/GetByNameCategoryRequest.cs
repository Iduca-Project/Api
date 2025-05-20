using MediatR;

namespace Iduca.Application.Features.Categories.GetByName;

public sealed record GetByNameCategoryRequest(
    string Name
) : IRequest<GetByNameCategoryResponse>;