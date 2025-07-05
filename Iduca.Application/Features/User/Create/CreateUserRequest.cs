using MediatR;

namespace Iduca.Application.Features.User.Create;

public sealed record CreateUserRequest(
    string Name,
    string Identity,
    string Email,
    string Password,
    bool IsAdmin,
    Guid ResponsibleId,
    Guid? CompanyId,
    string? Image,
    List<Guid>? Interests
) : IRequest<CreateUserResponse>;
