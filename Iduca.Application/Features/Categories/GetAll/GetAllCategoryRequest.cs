
using MediatR;

namespace Iduca.Application.Features.Categories.GetAll;

public sealed record GetAllCategoryRequest(
    int Page,
    int MaxItens,
    string? Name
) : IRequest<GetAllCategoryResponse>;
