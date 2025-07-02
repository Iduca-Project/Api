using Iduca.Domain.Common.Enums;

namespace Iduca.Domain.Objects;

public class SessionData
{
    public required Guid UserId { get; set; }
    public required Guid UserCompanyId { get; set; }
    public required UserRole Role { get; set; }
    public bool IsAdmin { get; set; } = false;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid CompanyId { get; set; }
}