namespace Iduca.Application.Features.Auth.Login;

public record LoginResponse(
    string Token,
    bool Admin,
    bool Manager,
    bool FirstAccess
);
