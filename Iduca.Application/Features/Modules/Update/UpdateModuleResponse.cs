namespace Iduca.Application.Features.Modules.Update;

public sealed record UpdateModuleResponse(
    Guid Id,
    string Name,
    string Description,
    int Index,
    Guid CourseId,
    DateTime UpdatedAt
);
