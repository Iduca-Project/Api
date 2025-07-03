using MediatR;

namespace Iduca.Application.Features.Auth.ForgotPassword;

public sealed record ForgotPasswordRequest(
    string Email
) : IRequest<ForgotPasswordResponse>;
