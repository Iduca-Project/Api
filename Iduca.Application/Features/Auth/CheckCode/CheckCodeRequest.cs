using MediatR;

namespace Iduca.Application.Features.Auth.CheckCode;

public sealed record CheckCodeRequest(
    string Email,
    string Code
) : IRequest<CheckCodeResponse>;
