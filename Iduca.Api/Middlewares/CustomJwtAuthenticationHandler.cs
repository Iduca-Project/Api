using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Iduca.Application.Contracts;

namespace Iduca.Api.Middlewares;

public class CustomJwtAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IAuthenticator _authenticator;

    public CustomJwtAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IAuthenticator authenticator) : base(options, logger, encoder, clock)
    {
        _authenticator = authenticator;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            // Extrair o token do header Authorization
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();
            
            // Extrair dados do usuário do token
            var userData = _authenticator.ExtractToken(token);
            
            if (userData == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("Token inválido"));
            }

            // Criar claims baseadas nos dados do usuário
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userData.UserId.ToString()),
                new Claim(ClaimTypes.Email, userData.Email),
                new Claim(ClaimTypes.Name, userData.Name),
                new Claim("CompanyId", userData.CompanyId.ToString()),
                new Claim("UserCompanyId", userData.UserCompanyId.ToString())
            };

            // Adicionar claim de admin se necessário
            if (userData.IsAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                claims.Add(new Claim("IsAdmin", "true"));
            }

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erro na autenticação do token");
            return Task.FromResult(AuthenticateResult.Fail("Erro na validação do token"));
        }
    }
}
