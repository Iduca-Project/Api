using MediatR;

namespace Iduca.Application.Features.User.Update;

public sealed record UpdateUserRequest(
    Guid Id,
    string Name,
    string Identity,
    string Email,
    bool IsAdmin,
    Guid? ResponsibleId,
    Guid CompanyId,
    string? Image,
    List<Guid> Interests
) : IRequest<UpdateUserResponse>;
