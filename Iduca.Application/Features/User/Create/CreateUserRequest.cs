using MediatR;

namespace Iduca.Application.Features.User.Create;

public sealed record CreateUserRequest(
    string Name,
    string Identity,
    string Email,
    string Password,
    Guid? CompanyId,
    Guid? ResponsibleId,
    bool? IsAdmin,
    string? Image,
    List<Guid>? Interests
) : IRequest<CreateUserResponse>;
