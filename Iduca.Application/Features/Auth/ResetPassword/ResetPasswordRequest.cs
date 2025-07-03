using MediatR;

namespace Iduca.Application.Features.Auth.ResetPassword;

public sealed record ResetPasswordRequest(
    string Email,
    string NewPassword
) : IRequest<ResetPasswordResponse>;
