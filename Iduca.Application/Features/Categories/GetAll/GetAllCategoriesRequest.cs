using MediatR;

namespace Iduca.Application.Features.Categories.GetAll;

public sealed record GetAllCategoriesRequest() : IRequest<List<GetAllCategoriesResponse>>;
