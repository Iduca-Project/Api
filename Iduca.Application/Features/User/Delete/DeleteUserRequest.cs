using MediatR;

namespace Iduca.Application.Features.User.Delete;

public sealed record DeleteUserRequest(Guid Id) : IRequest;
