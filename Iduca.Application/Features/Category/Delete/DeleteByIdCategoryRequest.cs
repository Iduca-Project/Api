using MediatR;

namespace Iduca.Application.Features.Categories.DeleteById;

public sealed record DeleteByIdCategoryRequest
(
    Guid Id
) : IRequest<DeleteByIdCategoryResponse>;