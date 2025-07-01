using MediatR;

namespace Iduca.Application.Features.User.Get;

public sealed record GetUserRequest(Guid Id) : IRequest<GetUserResponse>;
