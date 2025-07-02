namespace Iduca.Application.Features.Auth.Login;

public record LoginResponse(
    string Token,
    bool FirstAccess
);
