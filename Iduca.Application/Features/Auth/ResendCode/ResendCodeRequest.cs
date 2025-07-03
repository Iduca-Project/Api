using MediatR;

namespace Iduca.Application.Features.Auth.ResendCode;

public sealed record ResendCodeRequest(
    string Email
) : IRequest<ResendCodeResponse>;
