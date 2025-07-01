using MediatR;

namespace Iduca.Application.Features.User.GetByQuery;

public sealed record GetUsersRequest(
    string? Name,
    string? Email,
    Guid? CompanyId,
    bool? IsAdmin,
    int Page,
    int MaxItems
) : IRequest<List<GetUsersResponse>>;
