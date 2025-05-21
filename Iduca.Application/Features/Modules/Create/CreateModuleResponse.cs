using Iduca.Domain.Models;

namespace Iduca.Application.Features.Modules.Create;

public sealed record CreateModuleResponse(
    Guid Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? DisabledAt,
    string Name,
    string Description,
    Course Course
);