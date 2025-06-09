using Iduca.Domain.Models;
using Iduca.Domain.Objects;

namespace Iduca.Application.Contracts;

public interface IAuthenticator {
    string GenerateUserToken(User user);
    SessionData ExtractToken(string token);
}