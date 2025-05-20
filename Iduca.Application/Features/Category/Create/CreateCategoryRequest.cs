using MediatR;

namespace Iduca.Application.Features.Categories.Create;

public sealed record CreateCategoryRequest(
    string Name
) : IRequest<CreateCategoryResponse>;