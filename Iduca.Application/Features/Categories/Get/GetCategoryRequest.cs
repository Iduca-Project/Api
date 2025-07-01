using MediatR;

namespace Iduca.Application.Features.Categories.Get;

public sealed record GetCategoryRequest(Guid Id) : IRequest<GetCategoryResponse>;
