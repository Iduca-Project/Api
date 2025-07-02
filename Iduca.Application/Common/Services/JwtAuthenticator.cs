using Iduca.Application.Contracts;
using Iduca.Domain.Models;
using Iduca.Domain.Objects;
using Iduca.Domain.Common.Enums;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;

namespace Iduca.Application.Common.Services;

public class JwtAuthenticator : IAuthenticator
{
    private readonly IConfiguration _configuration;

    public JwtAuthenticator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateUserToken(User user)
    {
        // Por simplicidade, vamos criar um token básico
        // Em produção, use JWT com as dependências adequadas
        var tokenData = $"{user.Id}|{user.Email}|{user.IsAdmin}|{user.CompanyId}|{DateTime.UtcNow.AddHours(1):yyyy-MM-dd HH:mm:ss}";
        var tokenBytes = Encoding.UTF8.GetBytes(tokenData);
        return Convert.ToBase64String(tokenBytes);
    }

    public SessionData ExtractToken(string token)
    {
        try
        {
            var tokenBytes = Convert.FromBase64String(token);
            var tokenData = Encoding.UTF8.GetString(tokenBytes);
            var parts = tokenData.Split('|');

            if (parts.Length != 5)
                throw new ArgumentException("Invalid token format");

            var userId = Guid.Parse(parts[0]);
            var email = parts[1];
            var isAdmin = bool.Parse(parts[2]);
            var companyId = Guid.Parse(parts[3]);
            var expiryDate = DateTime.Parse(parts[4]);

            if (DateTime.UtcNow > expiryDate)
                throw new ArgumentException("Token expired");

            return new SessionData
            {
                UserId = userId,
                UserCompanyId = companyId,
                Role = isAdmin ? UserRole.Admin : UserRole.Reader,
                Name = string.Empty, // Será preenchido se necessário
                Email = email,
                IsAdmin = isAdmin,
                CompanyId = companyId
            };
        }
        catch
        {
            throw new ArgumentException("Invalid token");
        }
    }
}
