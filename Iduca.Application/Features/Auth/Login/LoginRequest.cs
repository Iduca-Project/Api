using MediatR;

namespace Iduca.Application.Features.Auth.Login;

public record LoginRequest(
    string Email,
    string Password
) : IRequest<LoginResponse>;
