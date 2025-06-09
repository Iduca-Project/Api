using System.Text.Json;
using Iduca.Application.Common.Session;
using Iduca.Application.Contracts;
using Iduca.Domain.Common.Messages;

namespace Iduca.API.Middlewares;

public class AuthenticateMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        var authHeader = context.Request.Headers.Authorization.FirstOrDefault();

        if (string.IsNullOrEmpty(authHeader))
        {
            await _next(context);
            return;
        }

        if (!authHeader.StartsWith("Bearer "))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            string message = JsonSerializer.Serialize(new { message = ExceptionMessage.Unauthorized.TokenPrefix });
            await context.Response.WriteAsync(message);
            return;
        }

        var token = authHeader["Bearer ".Length..];

        try
        {
            var authService = context.RequestServices.GetRequiredService<IAuthenticator>();
            var scopedSession = context.RequestServices.GetRequiredService<IRequestSession>();

            var sessionPayload = authService.ExtractToken(token);
            scopedSession.SetSession(sessionPayload);

            await _next(context);
        }
        catch(Exception e)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            string message = JsonSerializer.Serialize(new { message = e.Message });
            await context.Response.WriteAsync(message);
        }
    }
}